using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 远程武器类
/// 远程武器可以被玩家投掷出去
/// 玩家投掷出去的远程武器会在碰撞到怪物或者墙壁时消失
/// 远程武器的伤害和弓箭的伤害是一样的
/// 远程武器的攻击力是固定的
/// </summary>
/// <author>Xiao_Yanzhe</author>
/// <see cref="https://gitee.com/Xiao_pluto"/>
public class RemoteWeapons : Weapons
{
    private Transform remoteTransform;
    private SpriteRenderer spriteRenderer;
    private Vector3 initPos;
    private Vector3 maxPos;
    private Vector3 minPos;
    private List<GameObject> bads = new List<GameObject>();

    [Header("弹药的攻击距离和射击速度")]
    public float distance_ = 100f; //
    public float speed = 10f;

    [Header("特效")]
    public GameObject Fx;
    // Start is called before the first frame update
    void Start()
    {
        remoteTransform = this.GetComponent<Transform>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        initPos = remoteTransform.position;
        maxPos = new Vector3(initPos.x + distance_, initPos.y, initPos.z);
        minPos = new Vector3(initPos.x - distance_, initPos.y, initPos.z);
    }

    // Update is called once per frame
    void Update()
    {
        Move(spriteRenderer.flipX);
        if (bads.Count > 0)
        {
            Attack();
        }
    }

    private void Attack()
    {
        Bad bad = bads[0].GetComponent<Bad>();
        bad.TakeDamage(this.attackPower);
        if(Fx != null)
        {
            Instantiate(Fx, bad.transform.position, Quaternion.identity);
        }
        Destroy(this.gameObject);
        return;
    }

    private void Move(bool direction)
    {
        if (direction)
        {
            remoteTransform.position = Vector3.MoveTowards(remoteTransform.position, minPos, speed * Time.deltaTime);
        }
        else
        {
            remoteTransform.position = Vector3.MoveTowards(remoteTransform.position, maxPos, speed * Time.deltaTime);
        }
        if (Mathf.Abs(remoteTransform.position.x - initPos.x) >= distance_)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Monster" || collision.collider.tag == "Boss")
        {
            bads.Add(collision.gameObject);
        }
        if (collision.collider.tag == "Ground")
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Monster")
        {
            bads.Remove(collision.gameObject);
        }
    }
}