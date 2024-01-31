using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodNumManager : MonoBehaviour
{
    public Text current;

    //使用双重检查锁定尝试线程安全
    private static BloodNumManager instance = null;
    private static readonly object padlock = new object();
    private BloodNumManager() { }

    public static BloodNumManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BloodNumManager>();
                lock (padlock)
                {
                    if (instance == null)
                    {
                        GameObject bloodNumManagerObject = new GameObject("BloodNumManager");
                        instance = bloodNumManagerObject.AddComponent<BloodNumManager>();
                    }
                }
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //初始化血量数值信息
        current.text = "100";
    }

    public void BloodNumUpdate(int target) 
    {
        //根据目标血量数值更改血量数值
        current.text = target.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
