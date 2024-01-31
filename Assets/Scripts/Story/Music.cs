using UnityEngine;
/// <summary>
/// Music类用于控制游戏中的音乐。它确保音乐在场景之间持续播放。
/// </summary>
/// <author>Xiao_Yanzhe</author>
/// <see cref="https://gitee.com/Xiao_pluto"/>
public class Music : MonoBehaviour
{

    private static Music instance;

    /// <summary>
    /// 在对象被实例化时调用Awake方法。
    /// 它将此游戏对象的位置设置为MainCamera的位置，
    /// 并检查是否已经存在一个此对象的实例。
    /// 如果存在，则删除此游戏对象；
    /// 否则，将此游戏对象设置为实例，并确保在加载新的场景时不会销毁它。
    /// </summary>
    void Awake()
    {

        // 将此游戏对象的位置设置为MainCamera的位置

        transform.position = Camera.main.transform.position;

        // 检查是否已经存在一个此对象的实例
        // 如果存在，则删除此游戏对象
        // 否则，将此游戏对象设置为实例
        // 这用于Music游戏对象，以便音乐在场景之间持续播放

        if (instance != null && instance != this)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

}