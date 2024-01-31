using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] movePos;
    public float waitTime = 0.05f;
    public float speed;

    private int i;
    private Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        i = 1;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, movePos[i].position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, movePos[i].position) <= 0.1f)
        {
            if (waitTime < 0)
            {
                if (i == 1)
                {
                    i = 0;
                }
                else
                {
                    i = 1;
                }
                waitTime = 0.05f;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    //private void OnCollisionEnter2D(Collision2D other)
    //{
    //    if(other.gameObject.tag == "Player")
    //    {
    //        other.gameObject.transform.parent = gameObject.transform;
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        other.gameObject.transform.parent = playerTransform;
    //    }
    //}
}
