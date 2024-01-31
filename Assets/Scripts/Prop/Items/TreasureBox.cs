using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TreasureBox : Prop
{
    private bool canOpen;
    private bool isOpened;
    private Animator animator;
    private float forceMagnitude = 5f;
    [Header("会开出的武器或者道具")]
    public GameObject weapon1;
    public GameObject weapon2;
    public GameObject weapon3;
    public GameObject skill1;
    public GameObject skill2;
    public GameObject skill3;
    public GameObject skill4;
    public GameObject skill5;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isOpened = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.T)) 
        { 
            if(canOpen && !isOpened)
            {
                animator.SetTrigger("opening");
                PropGenerate();
                isOpened = true;
            }
        }*/
    }

/*    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")
            && collision.GetType().ToString() == "UnityEngine.BoxCollider2D")
        {
            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")
            && collision.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            canOpen = false;
        }
    }*/

    public override void PickedEffect()
    {
        if (!isOpened)
        {
            animator.SetTrigger("openning");
            PropGenerate();
            isOpened = true;
        }
    }

    public void PropGenerate()
    {
        //随机数量一个或者两个
        int num = Random.Range(1, 3);
        for (int i = 0; i <= num; i++)
        {
            //生成随机种类的道具
            int propType = Random.Range(1, 9);
            switch (propType)
            {
                case 1:
                    // 生成 weapon1
                    GameObject weapon1Item = Instantiate(weapon1, transform.position, Quaternion.identity);
                    ApplyRandomForce(weapon1Item.GetComponent<Rigidbody2D>());
                    DontDestroyOnLoad(weapon1Item);
                    break;
                case 2:
                    // 生成 weapon2
                    GameObject weapon2Item = Instantiate(weapon2, transform.position, Quaternion.identity);
                    ApplyRandomForce(weapon2Item.GetComponent<Rigidbody2D>());
                    DontDestroyOnLoad(weapon2Item);
                    break;
                case 3:
                    //生成 weapon3
                    GameObject weapon3Item = Instantiate(weapon3, transform.position, Quaternion.identity);
                    ApplyRandomForce(weapon3Item.GetComponent<Rigidbody2D>());
                    DontDestroyOnLoad(weapon3Item);
                    break;
                case 4:
                    // 生成 skill1
                    GameObject skill1Item = Instantiate(skill1, transform.position, Quaternion.identity);
                    ApplyRandomForce(skill1Item.GetComponent<Rigidbody2D>());
                    DontDestroyOnLoad(skill1Item);
                    break;
                case 5:
                    // 生成 skill2
                    GameObject skill2Item = Instantiate(skill2, transform.position, Quaternion.identity);
                    ApplyRandomForce(skill2Item.GetComponent<Rigidbody2D>());
                    DontDestroyOnLoad(skill2Item);
                    break;
                case 6:
                    // 生成 skill3
                    GameObject skill3Item = Instantiate(skill3, transform.position, Quaternion.identity);
                    ApplyRandomForce(skill3Item.GetComponent<Rigidbody2D>());
                    DontDestroyOnLoad(skill3Item);
                    break;
                case 7:
                    // 生成 skill4
                    GameObject skill4Item = Instantiate(skill4, transform.position, Quaternion.identity);
                    ApplyRandomForce(skill4Item.GetComponent<Rigidbody2D>());
                    break;
                case 8:
                    // 生成 skill5
                    GameObject skill5Item = Instantiate(skill5, transform.position, Quaternion.identity);
                    ApplyRandomForce(skill5Item.GetComponent<Rigidbody2D>());
                    DontDestroyOnLoad(skill5Item);
                    break;
            }
        }
    }

    /**
     * 添加随机力方法
     * 给掉落物一个随机的力，实现散射掉落
     */
    private void ApplyRandomForce(Rigidbody2D rb)
    {
        if (rb != null)
        {
            // 随机向上的瞬时力
            Vector2 randomUpwardForce = new Vector2(0f, Random.Range(forceMagnitude / 2f, forceMagnitude));
            rb.AddForce(randomUpwardForce, ForceMode2D.Impulse);

            // 随机水平方向的力
            Vector2 randomHorizontalForce = new Vector2(Random.Range(-forceMagnitude, forceMagnitude), 0f);
            rb.AddForce(randomHorizontalForce, ForceMode2D.Impulse);
        }
    }
}
