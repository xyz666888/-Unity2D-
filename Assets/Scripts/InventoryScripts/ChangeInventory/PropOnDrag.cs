using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// /// 道具拖拽类
/// 用于道具的拖拽
/// 实现了 IBeginDragHandler、IDragHandler、IEndDragHandler 接口
/// 通过实现这三个接口，实现了道具的拖拽功能
/// </summary>
/// <author>Xiao_Yanzhe</author>
/// <see cref="https://gitee.com/Xiao_pluto"/>
public class PropOnDrag : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    // 存储原始的父对象
    private Transform originalParent;

    /// <summary>
    /// 允许外部设置原始父对象
    /// </summary>
    public Transform OriginalParent
    {
        set => originalParent = value;
    }

    /// <summary>
    /// 在对象创建时，获取并存储原始的父对象
    /// </summary>
    private void Start()
    {
        originalParent = transform.parent;
    }

    /// <summary>
    /// 当开始拖拽时，将对象的父对象设置为原始父对象的父对象，并将对象的位置设置为鼠标的位置。
    /// 同时，禁止 CanvasGroup 组件阻挡射线，以便可以检测到鼠标的拖拽操作。
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 将对象的父对象设置为原始父对象的父对象
        transform.SetParent(originalParent.parent);
        // 将对象的位置设置为鼠标的位置
        transform.position = eventData.position;
        // 禁止 CanvasGroup 组件阻挡射线
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    /// <summary>
    /// 在拖拽过程中，将对象的位置更新为鼠标的位置。
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        // 将对象的位置更新为鼠标的位置
        transform.position = eventData.position;
    }

    /// <summary>
    /// 当拖拽结束时，根据拖拽的目标进行不同的操作。
    /// 如果拖拽的目标是 "Image(1)"、"Image(2)" 或 "Image"，则将对象移动到目标的父对象，并将目标移动到原始父对象。
    /// 同时，更新原始父对象，并允许 CanvasGroup 组件阻挡射线。
    /// 如果拖拽的目标不是这些，那么将对象移动回原始父对象，并允许 CanvasGroup 组件阻挡射线。
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        // 检查拖拽的目标是否是 "Image(1)"、"Image(2)" 或 "Image"
        if(eventData.pointerCurrentRaycast.gameObject.name == "Image(1)" || 
            eventData.pointerCurrentRaycast.gameObject.name == "Image(2)" ||
            eventData.pointerCurrentRaycast.gameObject.name == "Image")
        {
            // 将对象移动到目标的父对象
            transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.position;
            // 将目标移动到原始父对象
            eventData.pointerCurrentRaycast.gameObject.transform.position = originalParent.position;
            // 更新对象的父对象
            transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent);
            // 更新目标的父对象
            eventData.pointerCurrentRaycast.gameObject.transform.SetParent(originalParent);
            // 更新原始父对象
            this.originalParent = transform.parent;
            // 更新目标的原始父对象
            eventData.pointerCurrentRaycast.gameObject.GetComponent<PropOnDrag>().OriginalParent = eventData.pointerCurrentRaycast.gameObject.transform.parent;
            // 允许 CanvasGroup 组件阻挡射线
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            // 调用 ChangeInventory 的 ChangePlaer 方法
            ChangeInventory.Instance.ChangePlaer();
            return;
        }
        else
        {
            // 将对象移动回原始父对象
            transform.SetParent(originalParent);
            transform.position = originalParent.position;
            // 允许 CanvasGroup 组件阻挡射线
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
}