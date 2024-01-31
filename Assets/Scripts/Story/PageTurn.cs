/* 版权所有 (c) 2020 Kuneko. 保留所有权利. */

using UnityEngine;

/// <summary>
/// PageTurn类用于处理页签的点击事件。
/// 当点击页签时，它会翻书页。
/// </summary>
/// <author>Xiao_Yanzhe</author>
/// <see cref="https://gitee.com/Xiao_pluto"/>
public class PageTurn : MonoBehaviour
{


    public int direction; // 方向

    //----------------------------------------
    // 鼠标按下事件
    //----------------------------------------

    /// <summary>
    /// 当鼠标按下时调用此方法。
    /// 它会在此游戏对象被点击时翻书页。
    /// 如果在Unity编辑器中设置了方向，则将该方向发送到书本。
    /// 如果没有设置方向，则使用页签的名称作为目标页数，例如Tab01跳转到第1页。
    /// </summary>
    void OnMouseDown()
    {

        // 当此游戏对象被点击时翻书页
        // 如果在Unity编辑器中设置了方向，则将该方向发送到书本
        // 如果没有设置方向，则使用页签的名称作为目标页数，例如Tab01跳转到第1页

        if (direction == 0)
            SendMessageUpwards("TurnToPage", int.Parse(name.Substring(3, 2)));
        else
            SendMessageUpwards("TurnPage", direction);

    }

}