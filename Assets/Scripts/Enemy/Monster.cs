using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// /// ������
/// �������Ϊ�У�Ѳ�ߣ����������ˣ�����
/// ����������У�Ѫ����Ѳ���ٶȣ�������ȴʱ�䣬������Χ�������ٶȣ������˺�
/// �����״̬�У��Ƿ񹥻����Ƿ����ˣ��Ƿ�����
/// �����״̬���У�Ѳ�ߣ����������ˣ�����
/// ����Ķ����У�Ѳ�ߣ����������ˣ�����
/// ����ļ���У��������Ƿ���빥����Χ��������򹥻���ң�������������Ѳ��
/// �������ʱ�����������빥����Χ����׷�����
/// �����������ⷶΧ�������¿�ʼѲ��
/// �����Ѳ�߷�Χ�У�A,B
/// ����Ĺ�����Χ�У�distance
/// ����Ĺ�����ʽ�У���ս��Զ��
/// </summary>
/// <author>Xiao_Yanzhe</author>
/// <see cref="https://gitee.com/Xiao_pluto"/>
public class Monster : Bad
{
    [Header("��������")]
    public int blood;
    public float patrolSpeed;
    public float attackCooldown;
    public float attackRange;
    public float attackSpeed;
    public int attackDamage;
    public bool attackControl; // ���Ϊ�����ɫˤ������Ч�����ǿ

    [Header("Ѳ�߷�Χ[A,B]")]
    public Vector3 pointA;
    public Vector3 pointB;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    private bool isAttacking;
    private bool isHurt;
    private bool isGround;
    private bool isIdle = false; //�Ƿ�ֹ״̬��������Ӧ��ֹһ��ʱ�䣩
    private float checkPlayerInterval = 0.5f;

    private Coroutine patrolCoroutine;
    private Coroutine detectCoroutine;
    private Coroutine attackCoroutine;

    [Header("����ʱ��")]
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

        // ��ʼЭ�̣�������Э������
       detectCoroutine = StartCoroutine(DetectPlayer());
    }

    void Update()
    {
        Rigidbody2D rigidbody = this.GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// ������AB��֮������Ѳ��
    /// </summary>
    /// <returns></returns>
    IEnumerator Patrol()
    {
        if(!isGround)
            yield return null;
        animator.SetBool("Walk", true);
        // ���õ�ǰĿ���Ϊ��A
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
            print("��ʱ��Э����Ѳ��");
            if (this.transform.position.x < pointA.x || this.transform.position.x > pointB.x)
            {
                //�����ʱ���ﲻ��Ѳ�߷�Χ�ڣ���ֹͣѲ��
                patrolCoroutine = null;
                //��ʼ�ƶ���Ѳ�߷�Χ
                StartCoroutine(MoveToPatrolRange());
                animator.SetBool("Walk", true);

                yield break;
            }
            
            // ������ﵱǰλ�õ�Ŀ���ľ���
            float distance = Mathf.Abs(transform.position.x - targetPoint.x);

            // �������С��һ����ֵ������0.1�������л�Ŀ���
            if (distance < 0.1f)
            {
                targetPoint = targetPoint == pointA ? pointB : pointA;
                this.spriteRenderer.flipX = targetPoint == pointA ? false : true;

            }
            // �ý�ɫ��Ŀ����ƶ�
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, 0.015f * patrolSpeed);

            yield return null; // �ȴ�һ֡
        }


    }

    /// <summary>
    /// �����������Ѳ�߷�Χ���ƶ���Ѳ�߷�Χ��Ҫ���ø�Э��
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveToPatrolRange()
    {
        if (!isGround)
            yield return null;
        Vector3 center = (pointA + pointB) / 2;
        // �����ɫ��ǰλ�õ�Ѳ�߷�Χ�ľ���
        float distanceToPatrolRange = transform.position.x - center.x;
        //������ɫ�ĳ���
        FacingTarget(center);
        // �ý�ɫ��Ѳ�߷�Χ�ƶ�
        distanceToPatrolRange = Mathf.Abs(distanceToPatrolRange);
        while (distanceToPatrolRange > 0.1f)
        {
            print("��ʱ��Э���ǻָ�Ѳ��");
            transform.position = Vector3.MoveTowards(transform.position, center, patrolSpeed * 0.015f);
            distanceToPatrolRange = Mathf.Abs(transform.position.x - center.x);
            yield return null; // �ȴ�һ֡
        }
        // ��ɫ�Ѿ���Ѳ�߷�Χ�ڣ���ʼѲ��
        if(patrolCoroutine == null)
            patrolCoroutine = StartCoroutine(Patrol());
    }

    /// <summary>
    /// ���﹥����ң����ڹ�����Χ�ڳ�������
    /// </summary>
    /// <returns></returns>
    IEnumerator Attack()
    {
        if (!isGround)
            yield return null;
        isAttacking = true;
        // ���Ź�������
        animator.SetTrigger("Attack");
        print("��ʱ�ǹ���Э��");
        // ����ʱ��������ƶ�
        Transform target = player;

        // ��ȡ��ǰʱ��
        float startTime = Time.time;

        bool attacked = false;
        // �ڶ������ŵ�ʱ����һֱǰ��
        while (Time.time - startTime < attackAnimTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, attackSpeed * 0.015f);
            //���ϸ���player��λ��
            player = PlayerAttribute.Instance.transform;
            // �����һ��ڹ�����Χ�ڣ�����˺�
            if (Mathf.Abs(transform.position.x - player.position.x) <= 0.5f && PlayerAttribute.Instance.IsGround && !attacked)
            {
                attacked = true;
                player.GetComponent<PlayerAttribute>().Hurt(attackDamage, attackControl);
            }
            yield return null; // �ȴ���һ֡
        }

        animator.SetBool("Walk",false);
        // �ȴ�������ȴʱ��
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
        animator.SetBool("Walk", true);

    }

    /// <summary>
    /// �����ܵ��˺�
    /// </summary>
    /// <param name="damage"></param>
    public override void TakeDamage(int damage)
    {
        // ֹͣѲ��
        if (patrolCoroutine != null)
        {
            StopCoroutine(patrolCoroutine);
            patrolCoroutine = null;
            StopAllCoroutines();
        }

        // �������˶���
        StartCoroutine(Hurt());

        // ����Ѫ��
        blood -= damage;

        // ���Ѫ��С�ڵ���0���ݻ�������Ϸ����
        if (blood <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(Die());
        }
        //����Ѳ��
        //patrolCoroutine = StartCoroutine(Patrol());
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <returns></returns>
    IEnumerator Die()
    {
        if (!isGround)
            yield return null;
        print("����������Э��");
        // ������������
        animator.SetTrigger("Dead");
        // �ȴ������������
        yield return new WaitForSeconds(deadAnimTime);
        PropItem.Instance.PropGenerate(gameObject.transform.position);
        ScoreManager.Instance.SkullUpdate();
        // ������Ϸ����
        Destroy(gameObject);
    }

    /// <summary>
    /// �����ܵ��˺�
    /// </summary>
    /// <returns></returns>
    IEnumerator Hurt()
    {
        if (!isGround)
            yield return null;
        print("����������Э��");
        FacingTarget(player.position);
        animator.SetBool("Walk", false);
        isHurt = true;
        // �������˶���
        animator.SetTrigger("Hurt");

        // �ȴ����˶����������
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
    /// ������ø�Э�̣�����һ���ᷢ�����
    /// �����δ���빥������(attackRange)��Ҫ���ø�Э��
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveToPlayerAndAttack()
    {
        if (!isGround)
            yield return null;
        animator.SetBool("Walk", true); 
        print("�������ƶ�����Э��");
        Vector3 playerPosition = player.position;
        // �����ɫ��ǰλ�õ���ҵľ���
        float distanceToPlayer = Mathf.Abs(this.transform.position.x - playerPosition.x);

        // �����ɫ���ڹ�����Χ��
        while (distanceToPlayer > attackRange)
        {
            // �ý�ɫ������ƶ�
            transform.position = Vector3.MoveTowards(transform.position, playerPosition, attackSpeed * 0.015f);
            distanceToPlayer = Mathf.Abs(this.transform.position.x - playerPosition.x);
            yield return null; // �ȴ�һ֡
        }

        // ��ɫ�Ѿ��ڹ�����Χ�ڣ���ʼ����
        StartCoroutine(Attack());
    }

    /// <summary>
    /// �������Ƿ���빥����Χ��������򹥻���ң�������������Ѳ��
    /// �������ʱ�����������빥����Χ����׷�����
    /// �����������ⷶΧ�������¿�ʼѲ��
    /// </summary>
    /// <returns></returns>
    IEnumerator DetectPlayer()
    {
        if (!isGround)
            yield return null;
        while (true)
        {
            print("��ʱ�Ǽ��Э��");

            //�����ʱ��ֹ����Ϊ�棬�򱣳־�ֹ.
            //if (isIdle)
            //{
            //    StartCoroutine(Idle());
            //}

            //���ϸ���player��λ��
            player = PlayerAttribute.Instance.transform;
            // �������ҵľ���
            float distanceToPlayer = transform.position.x - player.position.x;
            // ��������㹻�Ҳ��ڹ���״̬�����𹥻�  
            if (Mathf.Abs(distanceToPlayer) <= attackRange && !isAttacking && !isHurt
                //�ж��Ƿ���ͬһˮƽ����
                && Mathf.Abs(this.transform.position.y - player.position.y) <= 2.0f)
            {
                FacingTarget(player.position);
                // ֹͣѲ��
                if (patrolCoroutine != null)
                {
                    StopCoroutine(patrolCoroutine);
                    patrolCoroutine = null;
                }
                player = PlayerAttribute.Instance.transform;
                if(attackCoroutine == null)
                    attackCoroutine = StartCoroutine(MoveToPlayerAndAttack());
            }
            // �����������ⷶΧ�����¿�ʼѲ��
            else if (Mathf.Abs(distanceToPlayer) > attackRange && patrolCoroutine == null)
            {
                patrolCoroutine = StartCoroutine(Patrol());
            }

            // �ȴ����ļ��
            yield return new WaitForSeconds(checkPlayerInterval);
        }
    }

    ///// <summary>
    ///// �����Idle�������Ϊtrueʱ������ɫ��ֹ��ԭ��һ��ʱ��֮���ٿ�ʼһϵ�л
    ///// </summary>
    ///// <returns></returns>
    //IEnumerator Idle()
    //{
    //    // ���isIdleΪtrue
    //    if (isIdle)
    //    {
    //        print("�����Ǿ�ֹЭ��");

    //        animator.SetBool("Walk", false);
    //        // ֹͣ���л
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

    //        // �ȴ�һ��ʱ��
    //        yield return new WaitForSeconds(idleAnimTime);
    //        isIdle = false;
    //        animator.SetBool("Walk", true);
    //        // ���¿�ʼ�
    //        // ����Ҫ����Ѳ��Э�̣������ʱû��⵽����Ļ����Զ���������Ѳ��Э�̡�
    //        patrolCoroutine = StartCoroutine(Patrol());
    //        //detectCoroutine = StartCoroutine(DetectPlayer());

    //        // ��isIdle��Ϊfalse
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
        //�����������
        if (collision.collider.tag == "Ground")
        {
            isGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //����뿪����
        if (collision.collider.tag == "Ground")
        {
            isGround = false;
        }
    }
}