using Assets.DeadCell.Scripts.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestory : MonoBehaviour
{
    private static DontDestory instance;
    public static DontDestory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DontDestory>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(DontDestory).ToString();
                    instance = obj.AddComponent<DontDestory>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
