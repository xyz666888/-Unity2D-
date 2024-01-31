using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// �����еĵ��߽���
/// �����ĵ���Ϊ�����ͼ���
/// ������ԭ���ǽ������еĵ����������еĵ��߽��н���
/// �����������еĵ��߻ᷢ���仯�������еĵ���Ҳ�ᷢ���仯
/// Ϊ�˸��õ�����ҿ��ƣ��������еĵ��߽���ͼ�񻯴���
/// ʹ��ҿ��Ը��õĽ��е��ߵĽ���
/// �����ĵ���Ϊ�����ͼ���
/// </summary>
/// <author>Xiao_Yanzhe</author>
/// <see cref="https://gitee.com/Xiao_pluto"/>
public class ChangeInventory : MonoBehaviour
{
    private Transform originalTransform;
    [Header("������Ӧ��Bag")]
    public Image[] imageParentItemsByWeapon = new Image[3];
    public TMP_Text textByWeapon;
    private Dictionary<string, Weapons> mapWeapon = new Dictionary<string, Weapons>();
    private Image[] imageChildrenItemsByWeapon = new Image[3];
    private string[] textItemsByWeapon = new string[3];
    [Header("���ܶ�Ӧ��Bag")]
    public Image[] imageParentItemsBySkill = new Image[3];
    public TMP_Text textBySkill;
    private Image[] imageChildrenItemsBySkill = new Image[3];
    private string[] textItemsBySkill = new string[3];
    private Dictionary<string, Skill> mapSkill = new Dictionary<string, Skill>();
    private static ChangeInventory instance;

    /// <summary>
    /// ���±����е�������Ϣ
    /// ��Ϊ�����Ѿ����ˣ���ʰȡ���µ���������Ҫ�������е�������֮���и���
    /// Ϊ������Ҹ��òٿ�
    /// ����ͼ�񻯽��湩���ʹ�á�
    /// </summary>
    /// <param name="weaponByChange">�򱳰������޷����뱳������Ҫ�������ڵ�������֮���и���</param>
    public void CreatNewItem(Weapons weaponByChange)
    {
        //�����еĸ����㼶��Ӧ�ĺ��Ӳ㼶���ó���
        //ԭ�������ֶ�һһ���ã���������bug���ɴ�ÿ�����¸��¸����㼶��Ӧ�ĵĺ��Ӳ㼶
        //�����Զ�����
        for(int i = 0; i < 3; i++)
        {
            imageChildrenItemsByWeapon[i] = imageParentItemsByWeapon[i].gameObject.transform.GetChild(0).GetComponent<Image>();
        }
        //���ó��������
        PlayerAttribute.Instance.WeaponBag.SetActive(true);
        //�����������Ӧ��Image���sprite�滻��������sprite
        imageChildrenItemsByWeapon[0].sprite = weaponByChange.GetComponent<SpriteRenderer>().sprite;
        imageChildrenItemsByWeapon[0].gameObject.transform.SetParent(imageParentItemsByWeapon[0].gameObject.transform);
        this.originalTransform = weaponByChange.transform;
        //Ϊӳ����Ӷ�Ӧ��������Ϊ���Ժ���Լ���ʱ�临�Ӷ�ֱ�Ӹ������������
        mapWeapon.Add(imageChildrenItemsByWeapon[0].gameObject.name, weaponByChange);
        //��������������Ϣ��ӵ���Ӧ��������
        textItemsByWeapon[0] = weaponByChange.description;
        //���������޸Ĵ����µ�������Ϣ���������޸ı����е�������Ϣ
        for (int i = 1; i <= 2; i++)
        {
            imageChildrenItemsByWeapon[i].sprite = PlayerAttribute.Instance.weapons[i - 1].GetComponent<SpriteRenderer>().sprite;
            imageChildrenItemsByWeapon[i].gameObject.transform.SetParent(imageParentItemsByWeapon[i].gameObject.transform);
            mapWeapon.Add(imageChildrenItemsByWeapon[i].gameObject.name, PlayerAttribute.Instance.weapons[i - 1]);
            textItemsByWeapon[i] = PlayerAttribute.Instance.weapons[i - 1].description;
        }
    }

    /// <summary>
    /// ����Map��Ӧ������һֱ�����ͬһ��������μ���
    /// ���±�����Ӧ��ӳ�䲻��
    /// �޷��޸�ԭ�е�����
    /// ���滻�����������������ɣ���Ϊ�޷�Destroy
    /// һ��Destroy��ᵼ��ָ���ΪҰָ�룬�޷��ٴε��ã����Ž��ǽ����������õ�Ļ����ʹ��
    /// </summary>
    public void RefeshWeapon()
    {
        Image child = imageParentItemsByWeapon[0].gameObject.transform.GetChild(0).GetComponent<Image>();
        Weapons _wea = Instantiate(mapWeapon[child.name], originalTransform.position, Quaternion.identity);
        _wea.GetComponent<SpriteRenderer>().sortingOrder = 1;
        mapWeapon = new Dictionary<string, Weapons>();
    }

    /// <summary>
    /// ���±����е�����/����������Ϣ
    /// </summary>
    /// <param name="index">�����������е�����</param>
    public void UpdateItemInfo(int index)
    {
        if(textByWeapon != null)
            this.textByWeapon.text = this.textItemsByWeapon[index];
        else if(textBySkill != null)
            this.textBySkill.text = this.textItemsBySkill[index];
    }

    /// <summary>
    /// �޸������е�����������Ϣ
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
    /// ���±����еļ�����Ϣ
    /// ��Ϊ�����Ѿ����ˣ���ʰȡ���µļ��ܣ���Ҫ�������еļ�����֮���и���
    /// Ϊ������Ҹ��òٿ�
    /// ����ͼ�񻯽��湩���ʹ�á�
    /// </summary>
    /// <param name="skillByChange">�򱳰������޷����뱳������Ҫ�������ڵļ�����֮���и���</param>
    public void CreatNewItem(Skill skillByChange)
    {
        //�����еĸ����㼶��Ӧ�ĺ��Ӳ㼶���ó���
        //ԭ�������ֶ�һһ���ã���������bug���ɴ�ÿ�����¸��¸����㼶��Ӧ�ĵĺ��Ӳ㼶
        //�����Զ�����
        for (int i = 0; i < 3; i++)
        {
            imageChildrenItemsBySkill[i] = imageParentItemsBySkill[i].gameObject.transform.GetChild(0).GetComponent<Image>();
        }
        //���ó��������
        PlayerAttribute.Instance.SkillBag.SetActive(true);
        //�����������Ӧ��Image���sprite�滻�ɼ��ܵ�sprite
        imageChildrenItemsBySkill[0].sprite = skillByChange.GetComponent<SpriteRenderer>().sprite;
        imageChildrenItemsBySkill[0].gameObject.transform.SetParent(imageParentItemsBySkill[0].gameObject.transform);
        this.originalTransform = skillByChange.transform;
        //Ϊӳ����Ӷ�Ӧ�ļ��ܣ�Ϊ���Ժ���Լ���ʱ�临�Ӷ�ֱ�Ӹ�������ļ���
        mapSkill.Add(imageChildrenItemsBySkill[0].gameObject.name, skillByChange);
        //�����ܵ�������Ϣ��ӵ���Ӧ��������
        textItemsBySkill[0] = skillByChange.description;
        //���������޸Ĵ����µļ�����Ϣ���������޸ı����еļ�����Ϣ
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
    /// ����ģʽ
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
