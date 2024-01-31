using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 背包中的道具交换
/// 交换的道具为武器和技能
/// 交换的原理是将背包中的道具与人物中的道具进行交换
/// 交换后人物中的道具会发生变化，背包中的道具也会发生变化
/// 为了更好的让玩家控制，将背包中的道具进行图像化处理
/// 使玩家可以更好的进行道具的交换
/// 交换的道具为武器和技能
/// </summary>
/// <author>Xiao_Yanzhe</author>
/// <see cref="https://gitee.com/Xiao_pluto"/>
public class ChangeInventory : MonoBehaviour
{
    private Transform originalTransform;
    [Header("武器对应的Bag")]
    public Image[] imageParentItemsByWeapon = new Image[3];
    public TMP_Text textByWeapon;
    private Dictionary<string, Weapons> mapWeapon = new Dictionary<string, Weapons>();
    private Image[] imageChildrenItemsByWeapon = new Image[3];
    private string[] textItemsByWeapon = new string[3];
    [Header("技能对应的Bag")]
    public Image[] imageParentItemsBySkill = new Image[3];
    public TMP_Text textBySkill;
    private Image[] imageChildrenItemsBySkill = new Image[3];
    private string[] textItemsBySkill = new string[3];
    private Dictionary<string, Skill> mapSkill = new Dictionary<string, Skill>();
    private static ChangeInventory instance;

    /// <summary>
    /// 更新背包中的武器信息
    /// 因为背包已经满了，现拾取到新的武器，需要将背包中的武器与之进行更换
    /// 为了让玩家更好操控
    /// 制作图像化界面供玩家使用。
    /// </summary>
    /// <param name="weaponByChange">因背包已满无法放入背包，需要将背包内的武器与之进行更换</param>
    public void CreatNewItem(Weapons weaponByChange)
    {
        //将所有的父级层级对应的孩子层级调用出来
        //原操作是手动一一调用，但后续有bug，干脆每次重新更新父级层级对应的的孩子层级
        //让其自动更新
        for(int i = 0; i < 3; i++)
        {
            imageChildrenItemsByWeapon[i] = imageParentItemsByWeapon[i].gameObject.transform.GetChild(0).GetComponent<Image>();
        }
        //调用出背包面板
        PlayerAttribute.Instance.WeaponBag.SetActive(true);
        //将背包面包对应的Image里的sprite替换成武器的sprite
        imageChildrenItemsByWeapon[0].sprite = weaponByChange.GetComponent<SpriteRenderer>().sprite;
        imageChildrenItemsByWeapon[0].gameObject.transform.SetParent(imageParentItemsByWeapon[0].gameObject.transform);
        this.originalTransform = weaponByChange.transform;
        //为映射添加对应的武器，为了以后可以减少时间复杂度直接更新人物的武器
        mapWeapon.Add(imageChildrenItemsByWeapon[0].gameObject.name, weaponByChange);
        //将武器的描述信息添加到对应的数组中
        textItemsByWeapon[0] = weaponByChange.description;
        //上述均是修改待更新的武器信息，下面是修改背包中的武器信息
        for (int i = 1; i <= 2; i++)
        {
            imageChildrenItemsByWeapon[i].sprite = PlayerAttribute.Instance.weapons[i - 1].GetComponent<SpriteRenderer>().sprite;
            imageChildrenItemsByWeapon[i].gameObject.transform.SetParent(imageParentItemsByWeapon[i].gameObject.transform);
            mapWeapon.Add(imageChildrenItemsByWeapon[i].gameObject.name, PlayerAttribute.Instance.weapons[i - 1]);
            textItemsByWeapon[i] = PlayerAttribute.Instance.weapons[i - 1].description;
        }
    }

    /// <summary>
    /// 更新Map对应，避免一直加造成同一个武器多次加入
    /// 导致报错，对应的映射不上
    /// 无法修改原有的武器
    /// 将替换下来的武器重新生成，因为无法Destroy
    /// 一旦Destroy则会导致指针成为野指针，无法再次调用，最优解是将该武器设置到幕后不再使用
    /// </summary>
    public void RefeshWeapon()
    {
        Image child = imageParentItemsByWeapon[0].gameObject.transform.GetChild(0).GetComponent<Image>();
        Weapons _wea = Instantiate(mapWeapon[child.name], originalTransform.position, Quaternion.identity);
        _wea.GetComponent<SpriteRenderer>().sortingOrder = 1;
        mapWeapon = new Dictionary<string, Weapons>();
    }

    /// <summary>
    /// 更新背包中的武器/技能描述信息
    /// </summary>
    /// <param name="index">武器在数组中的索引</param>
    public void UpdateItemInfo(int index)
    {
        if(textByWeapon != null)
            this.textByWeapon.text = this.textItemsByWeapon[index];
        else if(textBySkill != null)
            this.textBySkill.text = this.textItemsBySkill[index];
    }

    /// <summary>
    /// 修改人物中的武器技能信息
    /// </summary>
    public void ChangePlaer()
    {
        if(this.mapWeapon.Count != 0)
        {
            for (int i = 0; i <= 1; i++)
            {
                PlayerAttribute.Instance.weapons[i] = mapWeapon[imageParentItemsByWeapon[i + 1].gameObject.transform.GetChild(0).name];
            }
        }
        else if(this.mapSkill.Count != 0)
        {
            for (int i = 0; i <= 1; i++)
            {
                PlayerAttribute.Instance.skills[i] = mapSkill[imageParentItemsBySkill[i + 1].gameObject.transform.GetChild(0).name];
            }
        }
        PlayerAttribute.Instance.ChangeWeaponSkill();
    }

    /// <summary>
    /// 更新背包中的技能信息
    /// 因为背包已经满了，现拾取到新的技能，需要将背包中的技能与之进行更换
    /// 为了让玩家更好操控
    /// 制作图像化界面供玩家使用。
    /// </summary>
    /// <param name="skillByChange">因背包已满无法放入背包，需要将背包内的技能与之进行更换</param>
    public void CreatNewItem(Skill skillByChange)
    {
        //将所有的父级层级对应的孩子层级调用出来
        //原操作是手动一一调用，但后续有bug，干脆每次重新更新父级层级对应的的孩子层级
        //让其自动更新
        for (int i = 0; i < 3; i++)
        {
            imageChildrenItemsBySkill[i] = imageParentItemsBySkill[i].gameObject.transform.GetChild(0).GetComponent<Image>();
        }
        //调用出背包面板
        PlayerAttribute.Instance.SkillBag.SetActive(true);
        //将背包面包对应的Image里的sprite替换成技能的sprite
        imageChildrenItemsBySkill[0].sprite = skillByChange.GetComponent<SpriteRenderer>().sprite;
        imageChildrenItemsBySkill[0].gameObject.transform.SetParent(imageParentItemsBySkill[0].gameObject.transform);
        this.originalTransform = skillByChange.transform;
        //为映射添加对应的技能，为了以后可以减少时间复杂度直接更新人物的技能
        mapSkill.Add(imageChildrenItemsBySkill[0].gameObject.name, skillByChange);
        //将技能的描述信息添加到对应的数组中
        textItemsBySkill[0] = skillByChange.description;
        //上述均是修改待更新的技能信息，下面是修改背包中的技能信息
        for (int i = 1; i <= 2; i++)
        {
            imageChildrenItemsBySkill[i].sprite = PlayerAttribute.Instance.skills[i - 1].GetComponent<SpriteRenderer>().sprite;
            imageChildrenItemsBySkill[i].gameObject.transform.SetParent(imageParentItemsBySkill[i].gameObject.transform);
            mapSkill.Add(imageChildrenItemsBySkill[i].gameObject.name, PlayerAttribute.Instance.skills[i - 1]);
            textItemsBySkill[i] = PlayerAttribute.Instance.skills[i - 1].description;
        }
    }

    public void RefeshSkill()
    {
        Image child = imageParentItemsBySkill[0].gameObject.transform.GetChild(0).GetComponent<Image>();
        Skill _skill = Instantiate(mapSkill[child.name], originalTransform.position, Quaternion.identity);
        _skill.GetComponent<SpriteRenderer>().sortingOrder = 1;
        mapSkill = new Dictionary<string, Skill>();
    }

    /// <summary>
    /// 单例模式
    /// </summary>
    public static ChangeInventory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ChangeInventory>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(ChangeInventory).ToString();
                    instance = obj.AddComponent<ChangeInventory>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
