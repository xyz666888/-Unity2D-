using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    //三个属性初始值
    private int rageAttribute = 1;//残暴
    private int tacticalAttribute = 1;//战术
    private int survialAttribute = 1;//生存
    //杀敌
    private int skull = 0;
    //金币
    private int coin = 0;

    //界面文字部分
    public Text rage;//残暴数值
    public Text tactical;//战术数值
    public Text survial;//生存数值
    public Text skullNum;//人头数量
    public Text coinNum;//金币数量

    //使用双重检查锁定尝试线程安全
    private static ScoreManager instance = null;
    private static readonly object padlock = new object();
    private ScoreManager() { }

    public static ScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ScoreManager>();
                lock (padlock)
                {
                    if (instance == null)
                    {
                        GameObject scoreManagerObject = new GameObject("ScoreManager");
                        instance = scoreManagerObject.AddComponent<ScoreManager>();
                    }
                }
            }
            return instance;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //初始化各个数值
        rage.text = rageAttribute.ToString();
        tactical.text = tacticalAttribute.ToString();
        survial.text = survialAttribute.ToString();
        skullNum.text = skull.ToString();
        coinNum.text = coin.ToString();
    }

    //更新三种属性数值
    //参数采用列表获得人物卷轴即各个属性值，根据获得的列表更改各属性值
    public void AttributeUpdate(List<int> args)
    {
        //进行 null 检查
        if (args is null)
        {
            throw new ArgumentNullException(nameof(args));
        }
        //更改三种属性数值
        rage.text = args[0].ToString();
        tactical.text = args[1].ToString();
        survial.text = args[2].ToString();
    }

    //更新杀敌数值
    //无参数，调用直接进行杀敌数量自增
    public void SkullUpdate()
    {
        skull++;
        skullNum.text = skull.ToString();
    }

    //更新金币数值
    //通过传参获取人物目前金币数量，更新界面金币数量显示
    public void CoinUpdate(int newCoin)
    {
        coinNum.text = newCoin.ToString();
    }


    // Update is called once per frame
    void Update()
    {

    }
}
