using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Bad
{
    public Transform player;
    public bool isFlipped = false;

    public int attackDamage = 20;
    public int enragedAttackDamage = 40;

    public Vector3 attackOffset;
    public float attackRange = 1f;
    public LayerMask attackMask;

    public int health = 200;

    public GameObject deathEffect;

    public bool isInvulnerable = false;

    public override void TakeDamage(int damage)
    {
        if (isInvulnerable)
        {
            return;
        }
        health -= damage;

        if(health < 100)
        {
            GetComponent<Animator>().SetBool("isEnraged", true);
        }

        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //Instantiate(deathEffect, transform.position, Quaternion.identity);
        GetComponent<Animator>().SetTrigger("Death");
        PropItem.Instance.PropGenerate(this.transform.position,true);
    }

    public void LookAtPlayer()
    {

        if(transform.position.x > player.position.x)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if(transform.position.x < player.position.x)
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
    
    public void Attack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if(colInfo != null)
        {
            PlayerAttribute.Instance.Hurt(attackDamage, true);
        }
    }

    public void EnragedAttack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null)
        {
            PlayerAttribute.Instance.Hurt(enragedAttackDamage, true);
        }
    }
}
