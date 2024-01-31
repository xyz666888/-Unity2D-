using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����M���ڻ���������ʾС��ͼ���ٴΰ���ȡ��С��ͼ
/// </summary>
public class MiniMap : MonoBehaviour
{
    public GameObject miniMap;
    public GameObject mini_Map;

    private void Start()
    {
       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            IsShowMap();
        }
    }

    /// <summary>
    /// ����M����ʾС��ͼʱ��ԭλ��С��ͼ��ʧ
    /// </summary>
    private void IsShowMap()
    {
        miniMap.SetActive(!miniMap.activeSelf);
        DontDestroyOnLoad(miniMap);
        mini_Map.SetActive(!mini_Map.activeSelf);
        DontDestroyOnLoad(mini_Map);
    }
}

