using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeachUI : MonoBehaviour
{
    public GameObject teachPanel = null; //���
    private bool isStopped = false;

    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            StartCoroutine(PopMenu());
        }
    }

    /// <summary>
    /// �ж��Ƿ񵯳��˵�
    /// </summary>
    IEnumerator PopMenu()
    {
        if (!isStopped)
        {
            teachPanel.SetActive(true);
            yield return null;

            //�������ɼ�����ͣ
            //Cursor.visible = true;
            isStopped = true;
            Time.timeScale = 0;
        }
        else
        {
            teachPanel.SetActive(false);
            yield return null;

            //���������ʧ������
            //Cursor.visible = false;
            isStopped = false;
            Time.timeScale = 1;
        }
    }

}
