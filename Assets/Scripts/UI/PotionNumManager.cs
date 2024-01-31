using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionNumManager : MonoBehaviour
{
    private int num = 0;//Ѫƿ����
    public Text number;

    // Start is called before the first frame update
    void Start()
    {
        //��ʼ����������
        number.text = num.ToString();
    }

    //ʹ��˫�ؼ�����������̰߳�ȫ
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


    //�жϵ��������������
    public void PotionNumUpdate(int numUpdate)
    {
        //���ݵ�ǰ��������ֱ�Ӹ���
        //�����жϣ����õ������� 3������ 0
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
