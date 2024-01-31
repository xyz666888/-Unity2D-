using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeachUI : MonoBehaviour
{
    public GameObject teachPanel = null; //面板
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
    /// 判断是否弹出菜单
    /// </summary>
    IEnumerator PopMenu()
    {
        if (!isStopped)
        {
            teachPanel.SetActive(true);
            yield return null;

            //设置鼠标可见并暂停
            //Cursor.visible = true;
            isStopped = true;
            Time.timeScale = 0;
        }
        else
        {
            teachPanel.SetActive(false);
            yield return null;

            //设置鼠标消失并继续
            //Cursor.visible = false;
            isStopped = false;
            Time.timeScale = 1;
        }
    }

}
