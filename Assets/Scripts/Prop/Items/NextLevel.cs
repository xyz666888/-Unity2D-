using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    //下一关的门是否已开启
    public bool isOpened = true;
    public bool canOpen;
    //获取动画控制器，用于切换动画
    private Animator animator;
    //单例模式
    private static NextLevel instance;

    /**
     * 单例模式方法
     */
    public static NextLevel Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<NextLevel>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(PlayerAttribute).ToString();
                    instance = obj.AddComponent<NextLevel>();
                }
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //初始化
        //初始下一关的门不打开
        isOpened = true;
        canOpen = false;
        //获取动画控制器
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if(isOpened && canOpen)
            {
                StartCoroutine(GoToNextLevel());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canOpen = false;
        }
    }
    /**
     * 开启下一关方法
     * boss死亡时候调用，开启下一关
     * 例如
     * if(boss.hp<=0)
     * {
     *      NextLevel.Instance.OpenNextLevel();
     * }
     */
    public void OpenNextLevel()
    {
        isOpened = true;
        animator.SetTrigger("opening");
        Debug.Log("下一关卡入口已打开");
    }

    /**
     * 进入下一关
     * player进入判定范围且按F，调用
     * NextLevel.Instance.GoToNextLevel();
     */
    IEnumerator GoToNextLevel()
    {
        if (isOpened)
        {
            //需要保存的对象需要添加此段代码，或者在此添加，或者在需要保存的对象start中添加DontDestroyOnLoad(this);
            //不然在切换场景后对象中的属性无法保存
            //尚未测试单例模式是否无需此操作
            //切换场景
            DontDestroyOnLoad(PlayerAttribute.Instance);
            FadeCanvas.Instance.fade.DOFade(1, 1);
            yield return new WaitForSeconds(1);
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                PlayerAttribute.Instance.gameObject.transform.position = new Vector3(-11, -4, 0);

            }
            else if (SceneManager.GetActiveScene().buildIndex == 4)
            {
                PlayerAttribute.Instance.gameObject.transform.position = new Vector3(-10, -11, 0);
              
            }
            else if (SceneManager.GetActiveScene().buildIndex == 5)
            {
                PlayerAttribute.Instance.gameObject.transform.position = new Vector3(367, 1, 0);
   
            }
            FadeCanvas.Instance.isJump = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            
        }
    }
}
