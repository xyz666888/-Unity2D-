using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Զ��������
/// Զ���������Ա����Ͷ����ȥ
/// ���Ͷ����ȥ��Զ������������ײ���������ǽ��ʱ��ʧ
/// Զ���������˺��͹������˺���һ����
/// Զ�������Ĺ������ǹ̶���
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

    [Header("��ҩ�Ĺ������������ٶ�")]
    public float distance_ = 100f; //
    public float speed = 10f;

    [Header("��Ч")]
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