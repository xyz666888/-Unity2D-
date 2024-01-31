using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    //��һ�ص����Ƿ��ѿ���
    public bool isOpened = true;
    public bool canOpen;
    //��ȡ�����������������л�����
    private Animator animator;
    //����ģʽ
    private static NextLevel instance;

    /**
     * ����ģʽ����
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
        //��ʼ��
        //��ʼ��һ�ص��Ų���
        isOpened = true;
        canOpen = false;
        //��ȡ����������
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
     * ������һ�ط���
     * boss����ʱ����ã�������һ��
     * ����
     * if(boss.hp<=0)
     * {
     *      NextLevel.Instance.OpenNextLevel();
     * }
     */
    public void OpenNextLevel()
    {
        isOpened = true;
        animator.SetTrigger("opening");
        Debug.Log("��һ�ؿ�����Ѵ�");
    }

    /**
     * ������һ��
     * player�����ж���Χ�Ұ�F������
     * NextLevel.Instance.GoToNextLevel();
     */
    IEnumerator GoToNextLevel()
    {
        if (isOpened)
        {
            //��Ҫ����Ķ�����Ҫ��Ӵ˶δ��룬�����ڴ���ӣ���������Ҫ����Ķ���start�����DontDestroyOnLoad(this);
            //��Ȼ���л�����������е������޷�����
            //��δ���Ե���ģʽ�Ƿ�����˲���
            //�л�����
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
