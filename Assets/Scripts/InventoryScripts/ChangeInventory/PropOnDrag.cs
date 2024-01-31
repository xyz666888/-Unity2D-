using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// /// ������ק��
/// ���ڵ��ߵ���ק
/// ʵ���� IBeginDragHandler��IDragHandler��IEndDragHandler �ӿ�
/// ͨ��ʵ���������ӿڣ�ʵ���˵��ߵ���ק����
/// </summary>
/// <author>Xiao_Yanzhe</author>
/// <see cref="https://gitee.com/Xiao_pluto"/>
public class PropOnDrag : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    // �洢ԭʼ�ĸ�����
    private Transform originalParent;

    /// <summary>
    /// �����ⲿ����ԭʼ������
    /// </summary>
    public Transform OriginalParent
    {
        set => originalParent = value;
    }

    /// <summary>
    /// �ڶ��󴴽�ʱ����ȡ���洢ԭʼ�ĸ�����
    /// </summary>
    private void Start()
    {
        originalParent = transform.parent;
    }

    /// <summary>
    /// ����ʼ��קʱ��������ĸ���������Ϊԭʼ������ĸ����󣬲��������λ������Ϊ����λ�á�
    /// ͬʱ����ֹ CanvasGroup ����赲���ߣ��Ա���Լ�⵽������ק������
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // ������ĸ���������Ϊԭʼ������ĸ�����
        transform.SetParent(originalParent.parent);
        // �������λ������Ϊ����λ��
        transform.position = eventData.position;
        // ��ֹ CanvasGroup ����赲����
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    /// <summary>
    /// ����ק�����У��������λ�ø���Ϊ����λ�á�
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        // �������λ�ø���Ϊ����λ��
        transform.position = eventData.position;
    }

    /// <summary>
    /// ����ק����ʱ��������ק��Ŀ����в�ͬ�Ĳ�����
    /// �����ק��Ŀ���� "Image(1)"��"Image(2)" �� "Image"���򽫶����ƶ���Ŀ��ĸ����󣬲���Ŀ���ƶ���ԭʼ������
    /// ͬʱ������ԭʼ�����󣬲����� CanvasGroup ����赲���ߡ�
    /// �����ק��Ŀ�겻����Щ����ô�������ƶ���ԭʼ�����󣬲����� CanvasGroup ����赲���ߡ�
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        // �����ק��Ŀ���Ƿ��� "Image(1)"��"Image(2)" �� "Image"
        if(eventData.pointerCurrentRaycast.gameObject.name == "Image(1)" || 
            eventData.pointerCurrentRaycast.gameObject.name == "Image(2)" ||
            eventData.pointerCurrentRaycast.gameObject.name == "Image")
        {
            // �������ƶ���Ŀ��ĸ�����
            transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.position;
            // ��Ŀ���ƶ���ԭʼ������
            eventData.pointerCurrentRaycast.gameObject.transform.position = originalParent.position;
            // ���¶���ĸ�����
            transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent);
            // ����Ŀ��ĸ�����
            eventData.pointerCurrentRaycast.gameObject.transform.SetParent(originalParent);
            // ����ԭʼ������
            this.originalParent = transform.parent;
            // ����Ŀ���ԭʼ������
            eventData.pointerCurrentRaycast.gameObject.GetComponent<PropOnDrag>().OriginalParent = eventData.pointerCurrentRaycast.gameObject.transform.parent;
            // ���� CanvasGroup ����赲����
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            // ���� ChangeInventory �� ChangePlaer ����
            ChangeInventory.Instance.ChangePlaer();
            return;
        }
        else
        {
            // �������ƶ���ԭʼ������
            transform.SetParent(originalParent);
            transform.position = originalParent.position;
            // ���� CanvasGroup ����赲����
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
}