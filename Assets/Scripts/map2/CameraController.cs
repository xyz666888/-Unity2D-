using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting; // 这个命名空间似乎不被使用，可以删除
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

/// <summary>
/// 实现摄像机对人物的平滑跟随
/// </summary>
public class CameraController : MonoBehaviour
{
    // 目标对象
    [Header("目标对象")]
    public GameObject target;
    //摄像机死亡区域大小
    [Header("摄像机死亡区域大小")]
    public Vector2 focusAreaSize;
    //偏移量
    [Header("偏移量")]
    public Vector2 offset = new Vector2(0, 1);
    [Header("lookAheadDstX为人物所看水平方向在画面占据的距离\r\nlookSmoothTimeX为平滑过渡的时间")]
    //lookAheadDstX为人物所看水平方向在画面占据的距离
    //lookSmoothTimeX为平滑过渡的时间
    public float lookAheadDstX;
    public float lookSmoothTimeX;
    [Header("lookAheadDstX为人物所看竖直方向在画面占据的距离\r\nlookSmoothTimeY为平滑过渡的时间")]
    //lookAheadDstX为人物所看竖直方向在画面占据的距离
    //lookSmoothTimeY为平滑过渡的时间
    //verticalSmoothTime为人物静止时按上下摄像头平滑移动的时间
    public float lookAheadDstY;
    public float lookSmoothTimeY;
    public float verticalSmoothTime;

    private FocusArea focusArea;            //摄像机死亡区域
    private float currentLookAheadX;        
    private float targetLookAheadX;
    private float lookAheadDirX;            //人物所看水平方向
    private float smoothLookVelocityX;
    private bool lookAheadStopped;          //判断水平移动是否停止（改变方向也算停止）

    private float currentLookAheadY;
    private float targetLookAheadY;
    private float smoothLookVelocityY;
    private float smoothVelocityY;

    //lastY为人物静止时按上下前的竖直位置
    private float lastY;                
    private bool isLookAheadY;

    //摄像机最小最大位置
    public Vector2 minPosition;
    public Vector2 maxPosition;
    // Start is called before the first frame update
    void Start()
    {
        // 在Start中初始化目标对象
        target = GameObject.FindGameObjectWithTag("Player");
        // 修正FocusArea的初始化
        focusArea = new FocusArea(target.GetComponent<BoxCollider2D>().bounds, focusAreaSize);
    }

    private void LateUpdate()
    {
        // 跟随
        focusArea.Update(target.GetComponent<BoxCollider2D>().bounds);      

        // 偏移
        Vector2 focusPosition = focusArea.center + offset;

        // 水平方向的控制
        if (focusArea.velocity.x != 0)
        {
            // 获取水平方向速度的符号，即玩家所看方向
            lookAheadDirX = Mathf.Sign(focusArea.velocity.x);

            // 当玩家输入方向与移动方向一致时，计算摄像机移动的目标距离
            if (Input.GetAxis("Horizontal") != 0 && Mathf.Sign(Input.GetAxis("Horizontal")) == Mathf.Sign(focusArea.velocity.x))
            {
                lookAheadStopped = false;
                targetLookAheadX = lookAheadDirX * lookAheadDstX;
            }
            else
            {
                // 当玩家停止输入或改变方向时，摄像机移动的目标位置略微移动
                if (!lookAheadStopped)
                {
                    targetLookAheadX = currentLookAheadX + (lookAheadDirX * lookAheadDstX - currentLookAheadX) / 4;
                    lookAheadStopped = true;
                }
            }
        }
        //currentLookAheadX是当前移动的距离（平滑过渡），targetLookAheadX为目标移动距离
        //SmoothDamp是一个移动阻尼函数，可以实现平滑移动的效果
        currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);
        focusPosition += Vector2.right * currentLookAheadX;

        // 竖直方向的控制
        //target.CurrentVelocity.magnitude < 0.01f &&
        if (Input.GetAxis("Vertical") != 0)
        {
            if (!isLookAheadY)
            {
                //记录移动前位置，用于复位
                lastY = transform.position.y;
                isLookAheadY = true;
            }
            //实现上下的摄像机移动，同样使用SmoothDamp阻尼函数
            targetLookAheadY = Input.GetAxis("Vertical") * lookAheadDstY;
            currentLookAheadY = Mathf.SmoothDamp(currentLookAheadY, targetLookAheadY, ref smoothLookVelocityY, lookSmoothTimeY);
            focusPosition += Vector2.up * currentLookAheadY;
        }
        else
        {
            // 当玩家移动或停止输入竖直方向时，逐渐复位竖直偏移
            if (isLookAheadY)
            {
                currentLookAheadY = 0;
                focusPosition.y = lastY;
                isLookAheadY = false;
            }
            focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);
        }

        // 更新摄像机位置
        transform.position = (Vector3)focusPosition + Vector3.forward * -10;

        //限制摄像机范围
        Mathf.Clamp(transform.position.x, minPosition.x, maxPosition.x);
        Mathf.Clamp(transform.position.y, minPosition.y, maxPosition.y);
    }

    //调用bounds的边框绘制
    void OnDrawGizmos()
    {
        focusArea.DrawBounds();
    }

    /// <summary>
    /// 用于设置摄像机最小最大的位置，限制摄像机范围
    /// </summary>
    public void SetPosLimit(Vector2 minPos, Vector2 maxPos)
    {
        minPosition = minPos;
        maxPosition = maxPos;
    }
}

/// <summary>
/// 定义一个结构体，用来表示跟随框,即摄像机死亡区域
/// </summary>
public struct FocusArea
{
    // 中心
    public Vector2 center;
    // l为左边，r为右边，t为顶部，b为底部
    float l, r, t, b;

    // 速度
    public Vector2 velocity;

    public FocusArea(Bounds targetBounds, Vector2 focusAreaSize)
    {
        // 修正center的计算
        l = targetBounds.min.x - focusAreaSize.x;
        r = targetBounds.max.x + focusAreaSize.x;
        t = targetBounds.min.y + focusAreaSize.y;
        b = targetBounds.min.y;

        center = new Vector2((l + r) / 2, (t + b) / 2);

        velocity = Vector2.zero;
    }

    public void Update(Bounds targetBounds)
    {
        float shiftX = 0;
        // 当目标超出左边界时，计算需要移动的距离
        if (targetBounds.min.x < l)
        {
            shiftX = targetBounds.min.x - l;
        }
        // 当目标超出右边界时，计算需要移动的距离
        else if (targetBounds.max.x > r)
        {
            shiftX = targetBounds.max.x - r;
        }

        l += shiftX;
        r += shiftX;

        float shiftY = 0;

        // 当目标超出底部边界时，计算需要移动的距离
        if (targetBounds.min.y < b)
        {
            shiftY = targetBounds.min.y - b;
        }
        // 当目标超出顶部边界时，计算需要移动的距离
        else if (targetBounds.max.y > t)
        {
            shiftY = targetBounds.max.y - t;
        }

        t += shiftY;
        b += shiftY;

        center = new Vector2((l + r) / 2, (t + b) / 2);
        velocity = new Vector2(shiftX, shiftY);
    }

    /// <summary>
    /// 显示bounds的边框，debug使用
    /// </summary>
    public void DrawBounds()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, new Vector3(r - l, t - b, 0));
    }

}
