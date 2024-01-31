using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.U2D;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    //����
    public Text weapon1Key;//����һ����
    public Text weapon2Key;//����������
    public GameObject img1;//ͼ�� 1
    public GameObject img2;//ͼ�� 2

    //ʹ��˫�ؼ�����������̰߳�ȫ
    private static WeaponManager instance = null;
    private static readonly object padlock = new object();
    private WeaponManager() { }

    public static WeaponManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<WeaponManager>();
                lock (padlock)
                {
                    if (instance == null)
                    {
                        GameObject weaponManagerObject = new GameObject("WeaponManager");
                        instance = weaponManagerObject.AddComponent<WeaponManager>();
                    }
                }
            }
            return instance;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //��ʼ�����������һ��������
        //δ��ȡ�����������ݲ���ʾ
        weapon1Key.text = "J";
        weapon2Key.text = string.Empty;
    }

    //���������б�
    //��������ͼ�ꡣ��ʾ����ʹ�ð���
    public void WeaponUpdate(List<Weapons> weapons)
    {
        if (weapons.Count == 0)
        {
            return;
        }
        else if (weapons.Count == 1)
        {
            img1.GetComponent<Image>().sprite = weapons[0].gameObject.GetComponent<SpriteRenderer>().sprite;
        }
        else
        {

            img1.GetComponent<Image>().sprite = weapons[0].gameObject.GetComponent<SpriteRenderer>().sprite;
            img2.GetComponent<Image>().sprite = weapons[1].gameObject.GetComponent<SpriteRenderer>().sprite;
            weapon2Key.text = "K";
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}

