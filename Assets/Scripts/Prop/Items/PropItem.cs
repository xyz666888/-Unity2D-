using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class PropItem : MonoBehaviour
{
    //�������Ѫƿ�����ᣬ���
    [Header("����")]
    //Ѫƿ
    public GameObject potion;
    //��ŭ���Ծ���
    public GameObject rageScroll;
    //ս�����Ծ���
    public GameObject tacticalScroll;
    //�������Ծ���
    public GameObject survialScroll;
    [Header("���Ԥ����")]
    //���
    public GameObject coin;
    [Header("ɢ��ʱ���õ�����С")]
    private float forceMagnitude = 5f;
    //����ģʽ
    private static PropItem instance;
    [Header("�ռ�")]
    public GameObject easterEggDiary;


    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
    /*
     * ����ģʽ����
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
     * ��������
     * ����ʱ�������������ߺ�����������
     * if(monster.hp<=0)
     * {
     *      PropItem.Instance.PropGenerate();
     * }
     */
    public void PropGenerate(Vector3 position, bool isBoss = false)
    {
        if (isBoss)
        {
            //�����ռ�
            GameObject diary = Instantiate(easterEggDiary, position, Quaternion.identity);
            ApplyRandomForce(diary.GetComponent<Rigidbody2D>());
            return;
        }

        //�����������ĵ���
        int propType = Random.Range(1, 5);
        switch (propType)
        {
            case 1:
                // ���� potion
                GameObject potionItem = Instantiate(potion, position, Quaternion.identity);
                ApplyRandomForce(potionItem.GetComponent<Rigidbody2D>());
                break;
            case 2:
                // ���� rageScroll
                GameObject rageScrollItem = Instantiate(rageScroll, position, Quaternion.identity);
                ApplyRandomForce(rageScrollItem.GetComponent<Rigidbody2D>());
                break;
            case 3:
                // ���� tacticalScroll
                GameObject tacticalScrollItem = Instantiate(tacticalScroll, position, Quaternion.identity);
                ApplyRandomForce(tacticalScrollItem.GetComponent<Rigidbody2D>());
                break;
            case 4:
                // ���� survial
                GameObject survialItem = Instantiate(survialScroll, position, Quaternion.identity);
                ApplyRandomForce(survialItem.GetComponent<Rigidbody2D>());
                break;
        }
        
        // ���� coin
        for (int i = 0; i < Random.Range(1, 4); i++)
        {
            GameObject coinItem = Instantiate(coin, position, Quaternion.identity);
            ApplyRandomForce(coinItem.GetComponent<Rigidbody2D>());
        }


    }

    /**
     * ������������
     * ��������һ�����������ʵ��ɢ�����
     */
    private void ApplyRandomForce(Rigidbody2D rb)
    {
        if (rb != null)
        {
            // ������ϵ�˲ʱ��
            Vector2 randomUpwardForce = new Vector2(0f, Random.Range(forceMagnitude / 2f, forceMagnitude));
            rb.AddForce(randomUpwardForce, ForceMode2D.Impulse);

            // ���ˮƽ�������
            Vector2 randomHorizontalForce = new Vector2(Random.Range(-forceMagnitude, forceMagnitude), 0f);
            rb.AddForce(randomHorizontalForce, ForceMode2D.Impulse);
        }
    }
}
