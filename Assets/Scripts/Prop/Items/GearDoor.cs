using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearDoor : MonoBehaviour
{
    //����������
    private Animator animator;
    //�ж��Ƿ񱻴�
    private bool isopened;
    //�����ȴ�ʱ��
    private int seconds;
    //ʹ��Э�̵�����
    private Coroutine openCoroutine;


    // Start is called before the first frame update
    void Start()
    {
        //��ʼ����������������־��������Ҫ�ȴ���ʱ��
        animator = GetComponent<Animator>();
        isopened = false;
        seconds = 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenGearDoor()
    {
        if(!isopened)
        {
            //���ſ��Ŷ���
            animator.SetTrigger("openning");
            openCoroutine = StartCoroutine(Opening());
        }
    }

    IEnumerator Opening()
    {
        //�ȴ������������
        yield return new WaitForSeconds(seconds);
        //�ݻٶ���
        Destroy(gameObject);
    }
}
