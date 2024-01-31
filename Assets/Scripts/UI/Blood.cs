using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    //使用双重检查锁定尝试线程安全
    private static Blood instance = null;
    private static readonly object padlock = new object();
    private Blood() { }

    public static Blood Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Blood>();
                lock (padlock)
                {
                    if (instance == null)
                    {
                        GameObject bloodObject = new GameObject("Blood");
                        instance = bloodObject.AddComponent<Blood>();
                    }
                }
            }
            return instance;
        }
    }

    //调用两个函数
    //进行血条变化动画和血量数值变化
    public void BloodSumUpdate(int target)
    {
        BloodManage.Instance.BloodUpdate(target);//血条动画
        BloodNumManager.Instance.BloodNumUpdate(target);//血条数值
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
