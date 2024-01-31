using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ��ҿ�����
/// ���ڿ�����ҵ��ƶ�����Ծ�������Ȳ���
/// ͨ�����ƶ���״̬����������Ҷ����Ĳ���
/// ��Ҫ���ܣ�
/// 1.������ҵ��ƶ�
/// 2.������ҵ���Ծ
/// 3.������ҵĹ���
/// 4.������ҵķ���
/// 5.������ҵļ���
/// 
/// �����������߼���
/// 1.��Update�м����ҵİ���
/// 2.��Update�ж���ҽ����ƶ�
/// 3.��Update�м����ҵĶ���״̬����������ҡ��ֹͣ����
/// 
/// ����������ע�����
/// 1.�ڼ����ҵĶ���״̬ʱ����������ҡ��ֹͣ����
/// </summary>
///  <author>Xiao_Yanzhe</author>
/// <see  cref="https://gitee.com/Xiao_pluto"/>
public class PlayerController : MonoBehaviour
{
    //�����������塢��Ⱦ��
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    //��ⰴ��״̬
    private bool isADKeyPressed = false;
    private bool isSKeyPressed = false;
    private bool isJKeyPressed = false;
    private bool isKKeyPressed = false;
    private bool isOpenBag = false;
    //ˮƽ������ٶȣ����ڲ��Ŷ������ٶȽ���/��������
    private float horizontal = 0.0f;
    private float timer = 0f;
    private float closeTimer1 = 0f;
    private float closeTimer2 = 0f;
    private float skillUCoolTimer = 0f;
    private float skillICoolTimer = 0f;
    //�����ٶ�����
    private float acceleration = 20f;
    private float maxSpeed = 8f;
    //����
    private Weapons weaponJ;
    private Weapons weaponK;

    //����
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
    ///     /// �ƶ�����
    /// ͨ�����Ƹ����ˮƽ�ٶ��������ƶ�
    /// ͨ�����ƶ���״̬�������ƶ����Ĳ���
    /// ��סshift�����Լ���
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
                // ���Ƹ����ˮƽ�ٶ�
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
        ////��Ϊ��סAʱ��ˮƽ�����horizontal��Ϊ��ֵ
        ////ȡ�����ֵ���ײ��Ŷ���
        horizontal = Mathf.Abs(horizontal);
        animator.SetFloat("Speed", horizontal);
    }

    /// <summary>
    /// ���Ŷ��¶�����ͬʱ�����ƶ�
    /// </summary>
    private void Squat()
    {
        // �����������Ӵ�ʱ�ſ��Զ��£���Ծʱ���ɶ���
        if (Input.GetKey(KeyCode.S) && PlayerAttribute.Instance.IsGround)
        {
            isSKeyPressed = true;
            //����ʱ�ٶȱ������ϳ�ʶ
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
    /// ������Ծ�Ķ���
    /// </summary>
    private void Jump()
    {
        //���뱣֤��ʱ�ڵ�����ͬʱ���Ƕ����״̬�ſ�����Ծ
        //����ʱ��Ծ�����ϳ���
        if (Input.GetKeyDown(KeyCode.Space) && PlayerAttribute.Instance.IsGround && !isSKeyPressed)
        {
            //�����ֱ���ϵ������ﵽ��ԾЧ��
            rb.AddForce(new Vector2(0, 200));
            animator.SetTrigger("Jump");
        }
    }

    /// <summary>
    /// ���
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
    ///���������ǰ����������
    /// </summary>
    private string GetAnimName()
    {
        //�����Ӧ�Ķ���ֵ
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);

        // ��ȡ��ǰ�Ķ�������
        string animationName = clipInfo[0].clip.name;

        return animationName;
    }

    ///<summary>
    ///���ڸ��¶����������еĶ�������
    ///</summary>
    ///<return>���ص�ǰ����״̬</return>
    private AnimatorStateInfo UpdateAnimStatus()
    {
        //����animTime
        //Mathf.Repeat:
        //����Repeat��animTime��ֵֻ����01֮��
        //����ȷ��AnimTime������ָ�ֵ���ߴ���1��ֵ
        //Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime
        //�����ǻ�ȡ��ǰ��0�㲥�ŵĶ���ʱ�䣬����ֵת����01֮��
        animator.SetFloat("AnimTime", Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f));
        return animator.GetCurrentAnimatorStateInfo(0);
    }


    /// <summary>
    /// ��J������һ������
    /// ��סJһֱ�������ɿ�Jֹͣ����
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
            //ֹͣ��������
            isJKeyPressed = false;
            animator.SetBool(weaponJ.weaponName, false);

        }
        // ��⶯��״̬
        AnimatorStateInfo stateInfo = UpdateAnimStatus();
        if (stateInfo.IsName(weaponJ.weaponName) && !isJKeyPressed)
        {
            // �����ҡ��������
            animator.SetBool(weaponJ.weaponName, false);
            timer = 0f;
        }
    }

    /// <summary>
    /// K������������еĻ�
    /// ��סK��ʼ�������ɿ�K��������
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
            //ֹͣ��������
            isKKeyPressed = false;
            animator.SetBool(weaponK.weaponName, false);

        }
        // ��⶯��״̬
        AnimatorStateInfo stateInfo = UpdateAnimStatus();
        if (stateInfo.IsName(weaponK.weaponName) && !isKKeyPressed)
        {
            // �����ҡ��������
            animator.SetBool(weaponK.weaponName, false);
            timer = 0f;
        }
    }

    /// <summary>
    /// Զ�̹����Ĳ���
    /// </summary>
    /// <param name="weapon">��������</param>
    /// <param name="_gb">��Ӧ��������Ϸ������Ҫ�Դ˽���ʵ����</param>
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
    /// �����ս����������
    /// ��ɫ�������״̬�Ҳ����ܵ��˺�
    /// ͬʱҲ���ܽ������˺��ƶ�
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
    /// ��ս�����Ĳ���
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
            //�����monsters��һ��GameObject���͵�����
            var monsters = SelectTarget(weapon, this.transform);
            if (monsters != null)
            {
                HurtMonsterClose(monsters, weapon);
            }
            
        }

        //����ǽ�ս��������סW�����Խ��������
        if (Input.GetKey(KeyCode.W))
        {
            closeTimer2 += Time.deltaTime;
            animator.SetBool("BlowUp",true);
            
            if (closeTimer2 >= 0.5f)
            {
                closeTimer2 = 0f;
                //�����monsters ��һ��GameObject���͵�����
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
    /// ���������ڽ�������ɣ�ֻ�����ڽ�ս����
    /// �����������ɡ�
    /// </summary>
    /// <param name="monster"></param>
    /// <return></return>
    private void BlowUp(GameObject monster)
    {
        Monster monster1 = monster.GetComponent<Monster>();
        //��monster1��һ�����ϵ���
        Rigidbody2D rigidbody = monster1.GetComponent<Rigidbody2D>();
        print(monster1.name);
        rigidbody.AddForce(new Vector2(0,300));
    }

    /// <summary>
    /// ����weapon�Ĺ������ͼ������˺������Թ�������˺�
    /// ע�⣺ֻ�����ڽ�ս������Զ��������Ҫʵ������������������
    /// </summary>
    /// <param name="monster">�������Ϸ�������ͣ���Ҫ����˺��Ĺ�����</param>
    /// <param name="weapon">�����������������ǽ�ս</param>
    private void HurtMonsterClose(GameObject[] bad, Weapons weapon)
    {
        //�����Զ������ֱ���˳�
        if (weapon.isRemote) return;
        if(weapon.attackType == "Single")
        {
            var bads = bad[0].GetComponent<Monster>();
            bads.TakeDamage(weapon.attackPower);
        }
        else if(weapon.attackType == "Group")
        {
            //��monster����ת����Monster���͵�����
            for(int i = 0; i < bad.Length; i++)
            {
                bad[i].GetComponent<Monster>().TakeDamage(weapon.attackPower);
            }            
        }
    }

    /// <summary>
    /// ѡ��Ŀ�귽��
    /// </summary>
    /// <param name="weapons">��������ѡ��Ŀ��ȡ������</param>
    /// <param name="transform">ѡ��ʱ�Ĳο��㣨һ�������������������ӵ���ߣ��£�</param>
    /// <returns></returns>
    private GameObject[] SelectTarget(Weapons weapons, Transform ownerTransform)
    {
        //1.ͨ�������ң�������û��tag���ʱ������
        //2.��tag���ͨ��tag�����ܸ�
        //����ͨ�����߲���(��������Ϊ�뾶)������������tag
        var colliders = Physics2D.OverlapCircleAll(ownerTransform.position, weapons.attackDistance);
        if (colliders == null || colliders.Length == 0)
        {
            return null;
        }
        //�ҳ����Ϊxx(��attackTag�е�)����������
        var allCanBeSelected = colliders.FindAll(c => (Array.IndexOf(weapons.attackTag, c.tag) >= 0));
        if (allCanBeSelected == null || allCanBeSelected.Length == 0)
        {
            return null;
        }

        //��ɸѡ����ɫ���Եĵ���
        //��������Linq��ѯ��Ч�ʱȽϵͣ����Ǵ�����
        allCanBeSelected = allCanBeSelected.Where(c => (c.gameObject.GetComponent<Transform>().position.x - 
            ownerTransform.position.x) * (spriteRenderer.flipX ? -1 : 1) > 0).ToArray();

        //���������������б���ToArray����
        //��Ϊ����������Ĳ����������飬��tmp��FindAll�еķ���������List������ҪתΪ����
        //Collider[] allCanBeSelected = new Collider[tmp.Count];
        //for (int i = 0; i < tmp.Count; i++)
        //{
        //    allCanBeSelected[i] = tmp[i];

        //}
        //���ݼ��ܹ�������ȷ�����ص�������Ŀ��
        switch (weapons.attackType)
        {
            case "Group"://Ⱥ���Ͷ�ѡ
                return allCanBeSelected.Select(a => a.gameObject).ToArray();

            case "Single"://������ѡ�����
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
    /// ʰȡ�����������������������δ�������������
    /// ����Ǽ�������뼼�ܿ�
    /// ��������ԣ���ŭ�����ɣ����棩���޸�����
    /// ����ǽ�������ӽ������
    /// �����Ѫƿ������Ѫ��
    /// </summary>
    private void Land()
    {
        //��F��ѡ��ʰȡ��Ʒ
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("Land");
            //��ȡ��ײ��������
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
            //�ƶ���ɫ
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