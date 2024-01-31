using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    //ʹ��˫�ؼ�����������̰߳�ȫ
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

    //������������
    //����Ѫ���仯������Ѫ����ֵ�仯
    public void BloodSumUpdate(int target)
    {
        BloodManage.Instance.BloodUpdate(target);//Ѫ������
        BloodNumManager.Instance.BloodNumUpdate(target);//Ѫ����ֵ
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
