using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 按下M键在画面中央显示小地图，再次按下取消小地图
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
    /// 按下M健显示小地图时，原位置小地图消失
    /// </summary>
    private void IsShowMap()
    {
        miniMap.SetActive(!miniMap.activeSelf);
        DontDestroyOnLoad(miniMap);
        mini_Map.SetActive(!mini_Map.activeSelf);
        DontDestroyOnLoad(mini_Map);
    }
}

