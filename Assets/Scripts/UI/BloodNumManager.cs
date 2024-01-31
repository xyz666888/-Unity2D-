using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodNumManager : MonoBehaviour
{
    public Text current;

    //ʹ��˫�ؼ�����������̰߳�ȫ
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
        //��ʼ��Ѫ����ֵ��Ϣ
        current.text = "100";
    }

    public void BloodNumUpdate(int target) 
    {
        //����Ŀ��Ѫ����ֵ����Ѫ����ֵ
        current.text = target.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
