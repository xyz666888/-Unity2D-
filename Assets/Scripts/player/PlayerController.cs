using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 玩家控制器
/// 用于控制玩家的移动、跳跃、攻击等操作
/// 通过控制动画状态机来控制玩家动画的播放
/// 主要功能：
/// 1.控制玩家的移动
/// 2.控制玩家的跳跃
/// 3.控制玩家的攻击
/// 4.控制玩家的防御
/// 5.控制玩家的技能
/// 
/// 本控制器的逻辑：
/// 1.在Update中检测玩家的按键
/// 2.在Update中对玩家进行移动
/// 3.在Update中检测玩家的动画状态，如果进入后摇则停止攻击
/// 
/// 本控制器的注意事项：
/// 1.在检测玩家的动画状态时，如果进入后摇则停止攻击
/// </summary>
///  <author>Xiao_Yanzhe</author>
/// <see  cref="https://gitee.com/Xiao_pluto"/>
public class PlayerController : MonoBehaviour
{
    //动画器、刚体、渲染器
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    //检测按键状态
    private bool isADKeyPressed = false;
    private bool isSKeyPressed = false;
    private bool isJKeyPressed = false;
    private bool isKKeyPressed = false;
    private bool isOpenBag = false;
    //水平方向的速度，用于播放动画和速度渐增/渐减控制
    private float horizontal = 0.0f;
    private float timer = 0f;
    private float closeTimer1 = 0f;
    private float closeTimer2 = 0f;
    private float skillUCoolTimer = 0f;
    private float skillICoolTimer = 0f;
    //人物速度属性
    private float acceleration = 20f;
    private float maxSpeed = 8f;
    //武器
    private Weapons weaponJ;
    private Weapons weaponK;

    //技能
    private Skill skillU;
    private Skill skillI;


    // Start is called before the first frame update
    void Start()
    {
        UpdateWeaponSkill();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        print(PlayerAttribute.Instance.IsGuard);
        print(GetAnimName());
        //if (weaponJ != null && weaponK != null &&
        //    GetAnimName() != weaponJ.weaponName+"Loop" && GetAnimName() != weaponK.weaponName+"Loop")
        {
            Move();
        }
        OpenBag();
        Squat();
        Jump();
        Sprint();
        UpdateAnimStatus();
        Guard();
        Land();
        ClimbLadder();
        RestoreBlood();
        if(weaponJ != null)
            AttackByKeyJ();
        if(skillU != null)
            SkillByKeyU();
        if(skillI != null)
            SkillByKeyI();
        if(weaponK != null)
            AttackByKeyK();
    }

    /// <summary>
    ///     /// 移动方法
    /// 通过控制刚体的水平速度来控制移动
    /// 通过控制动画状态机来控制动画的播放
    /// 按住shift键可以加速
    /// </summary>
    private void Move()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            isADKeyPressed = true;
            horizontal = Input.GetAxis("Horizontal");
            if (horizontal < 0f)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }

            if (Input.GetKey(KeyCode.LeftShift) && !isSKeyPressed)
            {
                // 限制刚体的水平速度
                horizontal *= 2;
                maxSpeed = Mathf.Abs(horizontal) * 0.9f;
            }
            else
            {
                maxSpeed = 1.2f * Mathf.Abs(horizontal);
            }

        }
        else if (isADKeyPressed)
        {
            horizontal = 0f;
            isADKeyPressed = false;
        }

        rb.AddForce(new Vector2(horizontal * acceleration, 0));
        float clampedVelocityX = Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed);
        ////Debug.Log("Clamped Velocity X: " + clampedVelocityX);
        rb.velocity = new Vector2(clampedVelocityX, rb.velocity.y);
        ////Mathf.Abs(horizontal):
        ////因为向按住A时，水平方向的horizontal会为负值
        ////取其绝对值容易播放动画
        horizontal = Mathf.Abs(horizontal);
        animator.SetFloat("Speed", horizontal);
    }

    /// <summary>
    /// 播放蹲下动画，同时可以移动
    /// </summary>
    private void Squat()
    {
        // 必须在与地面接触时才可以蹲下，跳跃时不可蹲下
        if (Input.GetKey(KeyCode.S) && PlayerAttribute.Instance.IsGround)
        {
            isSKeyPressed = true;
            //蹲下时速度变慢符合常识
            BoxCollider2D collider = this.GetComponent<BoxCollider2D>();
            collider.size = new Vector2(0.61f, 0.72f);
            collider.offset = new Vector2(0, 0.33f);
            horizontal /= 5;
            animator.SetBool("Squat", true);
        }
        else if (isSKeyPressed)
        {
            BoxCollider2D collider = this.GetComponent<BoxCollider2D>();
            collider.size = new Vector2(0.61f, 1.3f);
            collider.offset = new Vector2(0, 0.62f);
            animator.SetBool("Squat", false);
            isSKeyPressed = false;
        }
        animator.SetFloat("Speed", horizontal);

    }

    /// <summary>
    /// 播放跳跃的动画
    /// </summary>
    private void Jump()
    {
        //必须保证此时在地面上同时不是蹲起的状态才可以跳跃
        //蹲起时跳跃不符合常理
        if (Input.GetKeyDown(KeyCode.Space) && PlayerAttribute.Instance.IsGround && !isSKeyPressed)
        {
            //添加竖直向上的力，达到跳跃效果
            rb.AddForce(new Vector2(0, 200));
            animator.SetTrigger("Jump");
        }
    }

    /// <summary>
    /// 冲刺
    /// </summary>
    private void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerAttribute.Instance.IsGuard = true;
            if(spriteRenderer.flipX)
            {
                animator.SetTrigger("Sprint");
                Vector3 target = this.transform.position - new Vector3(3, 0, 0);
                this.transform.position = Vector3.Lerp(this.transform.position, target, 0.7f);
            }
            else
            {
                animator.SetTrigger("Sprint");
                Vector3 target = this.transform.position + new Vector3(3, 0, 0);
                this.transform.position = Vector3.Lerp(this.transform.position, target, 0.7f);
            }
            
            PlayerAttribute.Instance.IsGuard = false;
        }
    }

    ///<summary>
    ///用于输出当前动画的名称
    /// </summary>
    private string GetAnimName()
    {
        //输出对应的动画值
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);

        // 获取当前的动画名称
        string animationName = clipInfo[0].clip.name;

        return animationName;
    }

    ///<summary>
    ///用于更新动画控制器中的动画变量
    ///</summary>
    ///<return>返回当前动画状态</return>
    private AnimatorStateInfo UpdateAnimStatus()
    {
        //更新animTime
        //Mathf.Repeat:
        //用了Repeat，animTime的值只会在01之间
        //用以确保AnimTime不会出现负值或者大于1的值
        //Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime
        //这里是获取当前第0层播放的动画时间，并将值转化成01之间
        animator.SetFloat("AnimTime", Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f));
        return animator.GetCurrentAnimatorStateInfo(0);
    }


    /// <summary>
    /// 按J攻击，一定会有
    /// 按住J一直攻击，松开J停止攻击
    /// </summary>
    private void AttackByKeyJ()
    {
        if (Input.GetKey(KeyCode.J))
        {
            if (weaponJ.isRemote)
                RemoteAttack(weaponJ);
            else
            {
                CloseAttack(weaponJ);
            }
            isJKeyPressed = true;
        }
        else if (isJKeyPressed)
        {
            //停止攻击动画
            isJKeyPressed = false;
            animator.SetBool(weaponJ.weaponName, false);

        }
        // 检测动画状态
        AnimatorStateInfo stateInfo = UpdateAnimStatus();
        if (stateInfo.IsName(weaponJ.weaponName) && !isJKeyPressed)
        {
            // 进入后摇结束攻击
            animator.SetBool(weaponJ.weaponName, false);
            timer = 0f;
        }
    }

    /// <summary>
    /// K键攻击，如果有的话
    /// 按住K开始攻击，松开K结束攻击
    /// </summary>
    private void AttackByKeyK()
    {
        if (Input.GetKey(KeyCode.K))
        {
            if (weaponK.isRemote)
                RemoteAttack(weaponK);
            else
            {
                CloseAttack(weaponK);
            }
            isKKeyPressed = true;
        }
        else if (isKKeyPressed)
        {
            //停止攻击动画
            isKKeyPressed = false;
            animator.SetBool(weaponK.weaponName, false);

        }
        // 检测动画状态
        AnimatorStateInfo stateInfo = UpdateAnimStatus();
        if (stateInfo.IsName(weaponK.weaponName) && !isKKeyPressed)
        {
            // 进入后摇结束攻击
            animator.SetBool(weaponK.weaponName, false);
            timer = 0f;
        }
    }

    /// <summary>
    /// 远程攻击的操作
    /// </summary>
    /// <param name="weapon">武器属性</param>
    /// <param name="_gb">对应的武器游戏对象，需要对此进行实例化</param>
    private void RemoteAttack(Weapons weapon)
    {
        animator.SetBool(weapon.weaponName, true);
        if (GetAnimName() == weapon.weaponName + "Loop")
        {
            timer += Time.deltaTime;
            if (timer >= weapon.attackInterval)
            {
                Transform transform = this.GetComponent<Transform>();
                Vector3 Pos = transform.position;
                SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
                if (spriteRenderer.flipX)
                {
                    SpriteRenderer sprite   = weapon.remoteGameObject.GetComponent<SpriteRenderer>();
                    sprite.flipX = true;
                    Pos.x -= 0.82f;
                    Pos.y += 1.6f;
                }
                else
                {
                    SpriteRenderer sprite = weapon.remoteGameObject.GetComponent<SpriteRenderer>();
                    sprite.flipX = false;
                    Pos.x += 0.82f;
                    Pos.y += 1.6f;
                }
                Vector3 _Pos = new Vector3(Pos.x, Pos.y, Pos.z);
                Instantiate(weapon.remoteGameObject, _Pos, Quaternion.identity);
                timer = 0f;
            }
        }
    }

    

    /// <summary>
    /// 架起近战武器，防御
    /// 角色进入霸体状态且不会受到伤害
    /// 同时也不能进攻敌人和移动
    /// </summary>
    private void Guard()
    {
        if(weaponJ != null && weaponJ.isRemote == false || weaponK != null && weaponK.isRemote == false)
        {
            if (Input.GetKey(KeyCode.X))
            {
                animator.SetBool("Guard", true);
                PlayerAttribute.Instance.IsGuard = true;
            }
            else if (Input.GetKeyUp(KeyCode.X))
            {
                animator.SetBool("Guard", false);
                PlayerAttribute.Instance.IsGuard = false;
            }
        }

    }

    /// <summary>
    /// 近战攻击的操作
    /// </summary>
    /// <param name="weapon"></param>
    /// <return></return>
    private void CloseAttack(Weapons weapon)
    {
        animator.SetBool(weapon.weaponName, true);
        closeTimer1 += Time.deltaTime;
        if (closeTimer1 >= 0.667f)
        {
            closeTimer1 = 0f;
            //这里的monsters是一个GameObject类型的数组
            var monsters = SelectTarget(weapon, this.transform);
            if (monsters != null)
            {
                HurtMonsterClose(monsters, weapon);
            }
            
        }

        //如果是近战武器，按住W键可以将怪物击飞
        if (Input.GetKey(KeyCode.W))
        {
            closeTimer2 += Time.deltaTime;
            animator.SetBool("BlowUp",true);
            
            if (closeTimer2 >= 0.5f)
            {
                closeTimer2 = 0f;
                //这里的monsters 是一个GameObject类型的数组
                var monsters = SelectTarget(weapon, this.transform);
                if (monsters != null)
                {
                    BlowUp(monsters[0]);
                    HurtMonsterClose(monsters, weapon);
                }
            }
        }
        else
        {
            animator.SetBool("BlowUp", false);
        }
        
    }

    /// <summary>
    /// 本函数用于将怪物击飞，只适用于近战武器
    /// 即将怪物挑飞。
    /// </summary>
    /// <param name="monster"></param>
    /// <return></return>
    private void BlowUp(GameObject monster)
    {
        Monster monster1 = monster.GetComponent<Monster>();
        //对monster1有一个向上的力
        Rigidbody2D rigidbody = monster1.GetComponent<Rigidbody2D>();
        print(monster1.name);
        rigidbody.AddForce(new Vector2(0,300));
    }

    /// <summary>
    /// 根据weapon的攻击类型及攻击伤害，来对怪物造成伤害
    /// 注意：只适用于近战武器，远程武器需要实例化，本函数不适用
    /// </summary>
    /// <param name="monster">怪物的游戏对象类型，即要造成伤害的怪物们</param>
    /// <param name="weapon">发动攻击的武器，是近战</param>
    private void HurtMonsterClose(GameObject[] bad, Weapons weapon)
    {
        //如果是远程武器直接退出
        if (weapon.isRemote) return;
        if(weapon.attackType == "Single")
        {
            var bads = bad[0].GetComponent<Monster>();
            bads.TakeDamage(weapon.attackPower);
        }
        else if(weapon.attackType == "Group")
        {
            //将monster数组转换成Monster类型的数组
            for(int i = 0; i < bad.Length; i++)
            {
                bad[i].GetComponent<Monster>().TakeDamage(weapon.attackPower);
            }            
        }
    }

    /// <summary>
    /// 选择目标方法
    /// </summary>
    /// <param name="weapons">武器对象，选择目标取决于它</param>
    /// <param name="transform">选择时的参考点（一般就是以主角自身（武器拥有者）呗）</param>
    /// <returns></returns>
    private GameObject[] SelectTarget(Weapons weapons, Transform ownerTransform)
    {
        //1.通过射线找：在物体没有tag标记时这样找
        //2.有tag标记通过tag找性能高
        //在这通过射线查找(攻击距离为半径)，在扇形中用tag
        var colliders = Physics2D.OverlapCircleAll(ownerTransform.position, weapons.attackDistance);
        if (colliders == null || colliders.Length == 0)
        {
            return null;
        }
        //找出标记为xx(在attackTag中的)的所有物体
        var allCanBeSelected = colliders.FindAll(c => (Array.IndexOf(weapons.attackTag, c.tag) >= 0));
        if (allCanBeSelected == null || allCanBeSelected.Length == 0)
        {
            return null;
        }

        //再筛选本角色正对的敌人
        //这里用了Linq查询，效率比较低，但是代码简洁
        allCanBeSelected = allCanBeSelected.Where(c => (c.gameObject.GetComponent<Transform>().position.x - 
            ownerTransform.position.x) * (spriteRenderer.flipX ? -1 : 1) > 0).ToArray();

        //根本不用这样，列表有ToArray方法
        //因为数组助手类的参数都是数组，而tmp从FindAll中的返回类型是List，所以要转为数组
        //Collider[] allCanBeSelected = new Collider[tmp.Count];
        //for (int i = 0; i < tmp.Count; i++)
        //{
        //    allCanBeSelected[i] = tmp[i];

        //}
        //根据技能攻击类型确定返回单个或多个目标
        switch (weapons.attackType)
        {
            case "Group"://群攻就都选
                return allCanBeSelected.Select(a => a.gameObject).ToArray();

            case "Single"://单攻就选最近的
                var collider = ArrayHelper.GetMin(allCanBeSelected.ToArray(),
                    a => Vector3.Distance(ownerTransform.position, a.transform.position));
                return new GameObject[] { collider.gameObject };
        }
        return null;
    }

    private void SkillByKeyU()
    {
        skillUCoolTimer += Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.U) && skillUCoolTimer >= skillU.skillCool)
        {
            skillUCoolTimer = 0f;

            skillU.CastSkill();
        }
    }

    private void SkillByKeyI()
    {
        skillICoolTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.I) && skillICoolTimer >= skillI.skillCool)
        {
            skillICoolTimer = 0f;

            skillI.CastSkill();
        }
    }

    public void UpdateWeaponSkill()
    {
        if (PlayerAttribute.Instance.weapons.Count >= 1)
        {
            weaponJ = PlayerAttribute.Instance.weapons[0];
        }

        if (PlayerAttribute.Instance.weapons.Count == 2)
        {
            weaponK = PlayerAttribute.Instance.weapons[1];
        }
        if (PlayerAttribute.Instance.skills.Count >= 1)
        {
            skillU = PlayerAttribute.Instance.skills[0];
            skillUCoolTimer = skillU.skillCool;
        }
        if (PlayerAttribute.Instance.skills.Count == 2)
        {
            skillI = PlayerAttribute.Instance.skills[1];
            skillICoolTimer = skillI.skillCool;
        }
    }

    /// <summary>
    /// 拾取东西，如果是武器且武器池未满则加入武器池
    /// 如果是技能则加入技能库
    /// 如果是属性（暴怒，技巧，生存）则修改属性
    /// 如果是金币则增加金币数量
    /// 如果是血瓶则增加血量
    /// </summary>
    private void Land()
    {
        //按F来选择拾取物品
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("Land");
            //获取碰撞到的物体
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
            foreach (Collider2D collider in colliders)
            {
                print(collider.name);
                if (collider.tag != "Ground" && collider.tag != "Monster" && collider.tag != "Player" && collider.tag != "Wall")
                {
                    collider.gameObject.GetComponentInChildren<Prop>().PickedEffect();
                    if (collider.tag == "Weapon" || collider.tag == "Skill")
                        PlayerAttribute.Instance.ChangeWeaponSkill();
                }
            }
        }
    }

    private void OpenBag()
    {
        if (Input.GetKeyDown(KeyCode.B) && !isOpenBag)
        {
            isOpenBag = true;
            PlayerAttribute.Instance.inventory.OpenBag();
        }
        else if(Input.GetKeyDown(KeyCode.B) && isOpenBag)
        {
            isOpenBag = false;
            PlayerAttribute.Instance.inventory.CloseBag();
        }
    }

    private void LadderClimbFinish()
    {
        Transform transform = this.GetComponent<Transform>();
        Vector3 pos = new Vector3(1f,1.24f,0);
        transform.position += pos;
    }

    private void ClimbLadder()
    {
        if(PlayerAttribute.Instance.canClimb && Input.GetKey(KeyCode.W))
        {
            animator.speed = 1;
            //移动角色
            PlayerAttribute.Instance.isClimbing = true;
            animator.SetBool("Climb", true);
            rb.velocity = new Vector2(0, 3);
            print(rb.velocity);

        }
        else if(PlayerAttribute.Instance.canClimb && Input.GetKeyUp(KeyCode.W) && PlayerAttribute.Instance.isClimbing)
        {
            animator.speed = 0;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
        }
        if (PlayerAttribute.Instance.isClimbing == false && Input.GetKey(KeyCode.W))
        {
            animator.SetBool("Climb", false);
            rb.velocity = Vector2.zero;
            rb.velocity = Vector2.zero;
            rb.velocity = Vector2.zero;
        }
    }

    private void RestoreBlood()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Potion.PotionEffect();
        }
    }
}