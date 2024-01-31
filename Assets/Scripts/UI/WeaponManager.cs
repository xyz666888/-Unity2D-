using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.U2D;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    //引用
    public Text weapon1Key;//武器一按键
    public Text weapon2Key;//武器二按键
    public GameObject img1;//图像 1
    public GameObject img2;//图像 2

    //使用双重检查锁定尝试线程安全
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
        //初始化，获得武器一、武器二
        //未获取的武器按键暂不显示
        weapon1Key.text = "J";
        weapon2Key.text = string.Empty;
    }

    //传入武器列表
    //更改武器图标。显示武器使用按键
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

