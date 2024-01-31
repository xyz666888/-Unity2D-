using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class PropItem : MonoBehaviour
{
    //怪物掉落血瓶，卷轴，金币
    [Header("道具")]
    //血瓶
    public GameObject potion;
    //暴怒属性卷轴
    public GameObject rageScroll;
    //战术属性卷轴
    public GameObject tacticalScroll;
    //生存属性卷轴
    public GameObject survialScroll;
    [Header("金币预制体")]
    //金币
    public GameObject coin;
    [Header("散落时候用的力大小")]
    private float forceMagnitude = 5f;
    //单例模式
    private static PropItem instance;
    [Header("日记")]
    public GameObject easterEggDiary;


    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
    /*
     * 单例模式方法
     */
    public static PropItem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PropItem>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(PlayerAttribute).ToString();
                    instance = obj.AddComponent<PropItem>();
                }
            }
            return instance;
        }
    }

    /**
     * 死亡方法
     * 死亡时生成随机种类道具和随机数量金币
     * if(monster.hp<=0)
     * {
     *      PropItem.Instance.PropGenerate();
     * }
     */
    public void PropGenerate(Vector3 position, bool isBoss = false)
    {
        if (isBoss)
        {
            //生成日记
            GameObject diary = Instantiate(easterEggDiary, position, Quaternion.identity);
            ApplyRandomForce(diary.GetComponent<Rigidbody2D>());
            return;
        }

        //生成随机种类的道具
        int propType = Random.Range(1, 5);
        switch (propType)
        {
            case 1:
                // 生成 potion
                GameObject potionItem = Instantiate(potion, position, Quaternion.identity);
                ApplyRandomForce(potionItem.GetComponent<Rigidbody2D>());
                break;
            case 2:
                // 生成 rageScroll
                GameObject rageScrollItem = Instantiate(rageScroll, position, Quaternion.identity);
                ApplyRandomForce(rageScrollItem.GetComponent<Rigidbody2D>());
                break;
            case 3:
                // 生成 tacticalScroll
                GameObject tacticalScrollItem = Instantiate(tacticalScroll, position, Quaternion.identity);
                ApplyRandomForce(tacticalScrollItem.GetComponent<Rigidbody2D>());
                break;
            case 4:
                // 生成 survial
                GameObject survialItem = Instantiate(survialScroll, position, Quaternion.identity);
                ApplyRandomForce(survialItem.GetComponent<Rigidbody2D>());
                break;
        }
        
        // 生成 coin
        for (int i = 0; i < Random.Range(1, 4); i++)
        {
            GameObject coinItem = Instantiate(coin, position, Quaternion.identity);
            ApplyRandomForce(coinItem.GetComponent<Rigidbody2D>());
        }


    }

    /**
     * 添加随机力方法
     * 给掉落物一个随机的力，实现散射掉落
     */
    private void ApplyRandomForce(Rigidbody2D rb)
    {
        if (rb != null)
        {
            // 随机向上的瞬时力
            Vector2 randomUpwardForce = new Vector2(0f, Random.Range(forceMagnitude / 2f, forceMagnitude));
            rb.AddForce(randomUpwardForce, ForceMode2D.Impulse);

            // 随机水平方向的力
            Vector2 randomHorizontalForce = new Vector2(Random.Range(-forceMagnitude, forceMagnitude), 0f);
            rb.AddForce(randomHorizontalForce, ForceMode2D.Impulse);
        }
    }
}
