using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingDamageEnemy : MonoBehaviour
{
    public int skillPower;
    public float radius;
    public void DamageEnemy()
    {
        GameObject[] enemys = FindEnemy();
        foreach (GameObject enemy in enemys)
        {
            Bad b = enemy.GetComponent<Bad>();
            b.TakeDamage(skillPower);
        }
    }

    private GameObject[] FindEnemy()
    {
        var colliders = Physics2D.OverlapCircleAll(this.transform.position, radius);
        if (colliders == null || colliders.Length == 0)
        {
            return null;
        }
        List<GameObject> enemy = new List<GameObject>();
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag == "Monster")
            {
                enemy.Add(collider.gameObject);
            }
        }
        return enemy.ToArray();
    }
}
