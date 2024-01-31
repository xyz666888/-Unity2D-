using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class LightUp : MonoBehaviour
{
    public SpriteRenderer sp;
    public float alphaValue = 1f;
    public int num = 0;
    private Animator anim;
    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.GetType().ToString() == "UnityEngine.BoxCollider2D")
        {
            sp = GetComponent<SpriteRenderer>();
            sp.color = new UnityEngine.Color(sp.color.r, sp.color.g, sp.color.b, 1);
            anim.SetTrigger("IsLight");
            num++;
        }
    }

   
}
