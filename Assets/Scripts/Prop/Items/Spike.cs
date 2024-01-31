using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public int damage;
    public float damageCD;
    private Coroutine damageCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Damage();
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(Damage(collision));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Damage();
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    IEnumerator Damage(Collider2D collision)
    {  
        while (true)
        {
            yield return new WaitForSeconds(damageCD);
            collision.gameObject.GetComponent<PlayerAttribute>().Hurt(damage, true);
        }
        
    }
}
