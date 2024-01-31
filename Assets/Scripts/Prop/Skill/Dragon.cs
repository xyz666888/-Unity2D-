using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    public void DragonRage()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("Monster");
        enemy.GetComponent<Bad>().TakeDamage(1000);
    }
}
