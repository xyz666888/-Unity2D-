using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// MenuButton类用于处理菜单按钮的点击事件。
/// 当点击按钮时，它会加载与按钮同名的场景。
/// </summary>
/// <author>Xiao_Yanzhe</author>
/// <see cref="https://gitee.com/Xiao_pluto"/>
public class MenuButton : MonoBehaviour
{
    /// <summary>
    /// 当鼠标按下时调用此方法。
    /// 它会加载与此游戏对象同名的场景。
    /// </summary>
    void OnMouseDown()
    {

        // 当此对象被点击时
        // 加载与此游戏对象同名的场景

        SceneManager.LoadScene(name);

    }

}