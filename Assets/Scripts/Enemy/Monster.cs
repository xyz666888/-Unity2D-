using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// /// 怪物类
/// 怪物的行为有：巡逻，攻击，受伤，死亡
/// 怪物的属性有：血量，巡逻速度，攻击冷却时间，攻击范围，攻击速度，攻击伤害
/// 怪物的状态有：是否攻击，是否受伤，是否死亡
/// 怪物的状态机有：巡逻，攻击，受伤，死亡
/// 怪物的动画有：巡逻，攻击，受伤，死亡
/// 怪物的检测有：检测玩家是否进入攻击范围，如果是则攻击玩家，如果不是则继续巡逻
/// 攻击玩家时，如果玩家脱离攻击范围，则追击玩家
/// 如果玩家脱离检测范围，则重新开始巡逻
/// 怪物的巡逻范围有：A,B
/// 怪物的攻击范围有：distance
/// 怪物的攻击方式有：近战，远程
/// </summary>
/// <author>Xiao_Yanzhe</author>
/// <see cref="https://gitee.com/Xiao_pluto"/>
public class Monster : Bad
{
    [Header("怪物属性")]
    public int blood;
    public float patrolSpeed;
    public float attackCooldown;
    public float attackRange;
    public float attackSpeed;
    public int attackDamage;
    public bool attackControl; // 如果为真则角色摔倒控制效果会更强

    [Header("巡逻范围[A,B]")]
    public Vector3 pointA;
    public Vector3 pointB;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    private bool isAttacking;
    private bool isHurt;
    private bool isGround;
    private bool isIdle = false; //是否静止状态（攻击后应静止一段时间）
    private float checkPlayerInterval = 0.5f;

    private Coroutine patrolCoroutine;
    private Coroutine detectCoroutine;
    private Coroutine attackCoroutine;

    [Header("动画时间")]
    public float deadAnimTime;
    public float idleAnimTime;
    public float hurtAnimTime;
    public float attackAnimTime;


    void Start()
    {
        player = PlayerAttribute.Instance.transform;
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = false;
        this.animator = GetComponent<Animator>();
        isAttacking = false;
        isHurt = false;

        // 开始协程，并保存协程引用
       detectCoroutine = StartCoroutine(DetectPlayer());
    }

    void Update()
    {
        Rigidbody2D rigidbody = this.GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// 怪物在AB点之间来回巡逻
    /// </summary>
    /// <returns></returns>
    IEnumerator Patrol()
    {
        if(!isGround)
            yield return null;
        animator.SetBool("Walk", true);
        // 设置当前目标点为点A
        Vector3 targetPoint;
        if (this.spriteRenderer.flipX)
        {
            targetPoint = pointB;
        }else
        {
            targetPoint = pointA;
        }
        while (true)
        {
            print("此时的协程是巡逻");
            if (this.transform.position.x < pointA.x || this.transform.position.x > pointB.x)
            {
                //如果此时怪物不在巡逻范围内，则停止巡逻
                patrolCoroutine = null;
                //开始移动到巡逻范围
                StartCoroutine(MoveToPatrolRange());
                animator.SetBool("Walk", true);

                yield break;
            }
            
            // 计算怪物当前位置到目标点的距离
            float distance = Mathf.Abs(transform.position.x - targetPoint.x);

            // 如果距离小于一定的值（例如0.1），则切换目标点
            if (distance < 0.1f)
            {
                targetPoint = targetPoint == pointA ? pointB : pointA;
                this.spriteRenderer.flipX = targetPoint == pointA ? false : true;

            }
            // 让角色向目标点移动
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, 0.015f * patrolSpeed);

            yield return null; // 等待一帧
        }


    }

    /// <summary>
    /// 如果怪物脱离巡逻范围则移动到巡逻范围需要调用该协程
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveToPatrolRange()
    {
        if (!isGround)
            yield return null;
        Vector3 center = (pointA + pointB) / 2;
        // 计算角色当前位置到巡逻范围的距离
        float distanceToPatrolRange = transform.position.x - center.x;
        //调整角色的朝向
        FacingTarget(center);
        // 让角色向巡逻范围移动
        distanceToPatrolRange = Mathf.Abs(distanceToPatrolRange);
        while (distanceToPatrolRange > 0.1f)
        {
            print("此时的协程是恢复巡逻");
            transform.position = Vector3.MoveTowards(transform.position, center, patrolSpeed * 0.015f);
            distanceToPatrolRange = Mathf.Abs(transform.position.x - center.x);
            yield return null; // 等待一帧
        }
        // 角色已经在巡逻范围内，开始巡逻
        if(patrolCoroutine == null)
            patrolCoroutine = StartCoroutine(Patrol());
    }

    /// <summary>
    /// 怪物攻击玩家，并在攻击范围内持续攻击
    /// </summary>
    /// <returns></returns>
    IEnumerator Attack()
    {
        if (!isGround)
            yield return null;
        isAttacking = true;
        // 播放攻击动画
        animator.SetTrigger("Attack");
        print("此时是攻击协程");
        // 攻击时朝向玩家移动
        Transform target = player;

        // 获取当前时间
        float startTime = Time.time;

        bool attacked = false;
        // 在动画播放的时间里一直前进
        while (Time.time - startTime < attackAnimTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, attackSpeed * 0.015f);
            //不断更新player的位置
            player = PlayerAttribute.Instance.transform;
            // 如果玩家还在攻击范围内，造成伤害
            if (Mathf.Abs(transform.position.x - player.position.x) <= 0.5f && PlayerAttribute.Instance.IsGround && !attacked)
            {
                attacked = true;
                player.GetComponent<PlayerAttribute>().Hurt(attackDamage, attackControl);
            }
            yield return null; // 等待下一帧
        }

        animator.SetBool("Walk",false);
        // 等待攻击冷却时间
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
        animator.SetBool("Walk", true);

    }

    /// <summary>
    /// 怪物受到伤害
    /// </summary>
    /// <param name="damage"></param>
    public override void TakeDamage(int damage)
    {
        // 停止巡逻
        if (patrolCoroutine != null)
        {
            StopCoroutine(patrolCoroutine);
            patrolCoroutine = null;
            StopAllCoroutines();
        }

        // 播放受伤动画
        StartCoroutine(Hurt());

        // 减少血量
        blood -= damage;

        // 如果血量小于等于0，摧毁自身游戏对象
        if (blood <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(Die());
        }
        //继续巡逻
        //patrolCoroutine = StartCoroutine(Patrol());
    }

    /// <summary>
    /// 怪物死亡
    /// </summary>
    /// <returns></returns>
    IEnumerator Die()
    {
        if (!isGround)
            yield return null;
        print("现在是死亡协程");
        // 播放死亡动画
        animator.SetTrigger("Dead");
        // 等待动画播放完毕
        yield return new WaitForSeconds(deadAnimTime);
        PropItem.Instance.PropGenerate(gameObject.transform.position);
        ScoreManager.Instance.SkullUpdate();
        // 销毁游戏对象
        Destroy(gameObject);
    }

    /// <summary>
    /// 怪物受到伤害
    /// </summary>
    /// <returns></returns>
    IEnumerator Hurt()
    {
        if (!isGround)
            yield return null;
        print("现在是受伤协程");
        FacingTarget(player.position);
        animator.SetBool("Walk", false);
        isHurt = true;
        // 播放受伤动画
        animator.SetTrigger("Hurt");

        // 等待受伤动画播放完毕
        yield return new WaitForSeconds(hurtAnimTime);

        if (patrolCoroutine != null)
        {
            StopCoroutine(patrolCoroutine);
            patrolCoroutine = null;
        }

        if (detectCoroutine != null)
        {
            StopCoroutine(detectCoroutine);
            detectCoroutine = null;
        }
        if(attackCoroutine == null)
            yield return StartCoroutine(MoveToPlayerAndAttack());

        isHurt = false;
        detectCoroutine = StartCoroutine(DetectPlayer());
    }

    /// <summary>
    /// 如果调用该协程，怪物一定会发起进攻
    /// 如果还未进入攻击距离(attackRange)不要调用该协程
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveToPlayerAndAttack()
    {
        if (!isGround)
            yield return null;
        animator.SetBool("Walk", true); 
        print("现在是移动攻击协程");
        Vector3 playerPosition = player.position;
        // 计算角色当前位置到玩家的距离
        float distanceToPlayer = Mathf.Abs(this.transform.position.x - playerPosition.x);

        // 如果角色不在攻击范围内
        while (distanceToPlayer > attackRange)
        {
            // 让角色向玩家移动
            transform.position = Vector3.MoveTowards(transform.position, playerPosition, attackSpeed * 0.015f);
            distanceToPlayer = Mathf.Abs(this.transform.position.x - playerPosition.x);
            yield return null; // 等待一帧
        }

        // 角色已经在攻击范围内，开始攻击
        StartCoroutine(Attack());
    }

    /// <summary>
    /// 检测玩家是否进入攻击范围，如果是则攻击玩家，如果不是则继续巡逻
    /// 攻击玩家时，如果玩家脱离攻击范围，则追击玩家
    /// 如果玩家脱离检测范围，则重新开始巡逻
    /// </summary>
    /// <returns></returns>
    IEnumerator DetectPlayer()
    {
        if (!isGround)
            yield return null;
        while (true)
        {
            print("此时是检测协程");

            //如果此时静止变量为真，则保持静止.
            //if (isIdle)
            //{
            //    StartCoroutine(Idle());
            //}

            //不断更新player的位置
            player = PlayerAttribute.Instance.transform;
            // 检测与玩家的距离
            float distanceToPlayer = transform.position.x - player.position.x;
            // 如果距离足够且不在攻击状态，则发起攻击  
            if (Mathf.Abs(distanceToPlayer) <= attackRange && !isAttacking && !isHurt
                //判断是否在同一水平线上
                && Mathf.Abs(this.transform.position.y - player.position.y) <= 2.0f)
            {
                FacingTarget(player.position);
                // 停止巡逻
                if (patrolCoroutine != null)
                {
                    StopCoroutine(patrolCoroutine);
                    patrolCoroutine = null;
                }
                player = PlayerAttribute.Instance.transform;
                if(attackCoroutine == null)
                    attackCoroutine = StartCoroutine(MoveToPlayerAndAttack());
            }
            // 如果玩家脱离检测范围，重新开始巡逻
            else if (Mathf.Abs(distanceToPlayer) > attackRange && patrolCoroutine == null)
            {
                patrolCoroutine = StartCoroutine(Patrol());
            }

            // 等待检查的间隔
            yield return new WaitForSeconds(checkPlayerInterval);
        }
    }

    ///// <summary>
    ///// 如果当Idle这个变量为true时，本角色静止在原地一段时间之后再开始一系列活动
    ///// </summary>
    ///// <returns></returns>
    //IEnumerator Idle()
    //{
    //    // 如果isIdle为true
    //    if (isIdle)
    //    {
    //        print("现在是静止协程");

    //        animator.SetBool("Walk", false);
    //        // 停止所有活动
    //        if (patrolCoroutine != null)
    //        {
    //            StopCoroutine(patrolCoroutine);
    //            patrolCoroutine = null;
    //        }
    //        if (detectCoroutine != null)
    //        {
    //            StopCoroutine(detectCoroutine);
    //            detectCoroutine = null;
    //        }

    //        // 等待一段时间
    //        yield return new WaitForSeconds(idleAnimTime);
    //        isIdle = false;
    //        animator.SetBool("Walk", true);
    //        // 重新开始活动
    //        // 不需要重启巡逻协程，如果此时没检测到人物的话会自动重新启动巡逻协程。
    //        patrolCoroutine = StartCoroutine(Patrol());
    //        //detectCoroutine = StartCoroutine(DetectPlayer());

    //        // 将isIdle设为false
    //    }
    //}

    private void FacingTarget(Vector3 position)
    {
        if(transform.position.x > position.x)
        {
            this.spriteRenderer.flipX = false;
        }
        else
        {
            this.spriteRenderer.flipX = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //如果碰到地面
        if (collision.collider.tag == "Ground")
        {
            isGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //如果离开地面
        if (collision.collider.tag == "Ground")
        {
            isGround = false;
        }
    }
}