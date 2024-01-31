using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    //�������Գ�ʼֵ
    private int rageAttribute = 1;//�б�
    private int tacticalAttribute = 1;//ս��
    private int survialAttribute = 1;//����
    //ɱ��
    private int skull = 0;
    //���
    private int coin = 0;

    //�������ֲ���
    public Text rage;//�б���ֵ
    public Text tactical;//ս����ֵ
    public Text survial;//������ֵ
    public Text skullNum;//��ͷ����
    public Text coinNum;//�������

    //ʹ��˫�ؼ�����������̰߳�ȫ
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
        //��ʼ��������ֵ
        rage.text = rageAttribute.ToString();
        tactical.text = tacticalAttribute.ToString();
        survial.text = survialAttribute.ToString();
        skullNum.text = skull.ToString();
        coinNum.text = coin.ToString();
    }

    //��������������ֵ
    //���������б���������ἴ��������ֵ�����ݻ�õ��б���ĸ�����ֵ
    public void AttributeUpdate(List<int> args)
    {
        //���� null ���
        if (args is null)
        {
            throw new ArgumentNullException(nameof(args));
        }
        //��������������ֵ
        rage.text = args[0].ToString();
        tactical.text = args[1].ToString();
        survial.text = args[2].ToString();
    }

    //����ɱ����ֵ
    //�޲���������ֱ�ӽ���ɱ����������
    public void SkullUpdate()
    {
        skull++;
        skullNum.text = skull.ToString();
    }

    //���½����ֵ
    //ͨ�����λ�ȡ����Ŀǰ������������½�����������ʾ
    public void CoinUpdate(int newCoin)
    {
        coinNum.text = newCoin.ToString();
    }


    // Update is called once per frame
    void Update()
    {

    }
}
