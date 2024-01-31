using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionNumManager : MonoBehaviour
{
    private int num = 0;//血瓶数量
    public Text number;

    // Start is called before the first frame update
    void Start()
    {
        //初始化道具数量
        number.text = num.ToString();
    }

    //使用双重检查锁定尝试线程安全
    private static PotionNumManager instance = null;
    private static readonly object padlock = new object();
    private PotionNumManager() { }

    public static PotionNumManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PotionNumManager>();
                lock (padlock)
                {
                    if (instance == null)
                    {
                        GameObject potionNumManagerObject = new GameObject("PotionNumManager");
                        instance = potionNumManagerObject.AddComponent<PotionNumManager>();
                    }
                }
            }
            return instance;
        }
    }


    //判断道具数量增减情况
    public void PotionNumUpdate(int numUpdate)
    {
        //根据当前道具数量直接更改
        //进行判断，设置道具上限 3，下限 0
        if (numUpdate < 0)
        { 
            numUpdate = 0;
        }
        else if (numUpdate > 3)
        {
            numUpdate = 3;
        }
        number.text = numUpdate.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
