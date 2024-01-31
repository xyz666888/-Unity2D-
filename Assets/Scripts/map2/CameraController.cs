using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting; // ��������ռ��ƺ�����ʹ�ã�����ɾ��
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

/// <summary>
/// ʵ��������������ƽ������
/// </summary>
public class CameraController : MonoBehaviour
{
    // Ŀ�����
    [Header("Ŀ�����")]
    public GameObject target;
    //��������������С
    [Header("��������������С")]
    public Vector2 focusAreaSize;
    //ƫ����
    [Header("ƫ����")]
    public Vector2 offset = new Vector2(0, 1);
    [Header("lookAheadDstXΪ��������ˮƽ�����ڻ���ռ�ݵľ���\r\nlookSmoothTimeXΪƽ�����ɵ�ʱ��")]
    //lookAheadDstXΪ��������ˮƽ�����ڻ���ռ�ݵľ���
    //lookSmoothTimeXΪƽ�����ɵ�ʱ��
    public float lookAheadDstX;
    public float lookSmoothTimeX;
    [Header("lookAheadDstXΪ����������ֱ�����ڻ���ռ�ݵľ���\r\nlookSmoothTimeYΪƽ�����ɵ�ʱ��")]
    //lookAheadDstXΪ����������ֱ�����ڻ���ռ�ݵľ���
    //lookSmoothTimeYΪƽ�����ɵ�ʱ��
    //verticalSmoothTimeΪ���ﾲֹʱ����������ͷƽ���ƶ���ʱ��
    public float lookAheadDstY;
    public float lookSmoothTimeY;
    public float verticalSmoothTime;

    private FocusArea focusArea;            //�������������
    private float currentLookAheadX;        
    private float targetLookAheadX;
    private float lookAheadDirX;            //��������ˮƽ����
    private float smoothLookVelocityX;
    private bool lookAheadStopped;          //�ж�ˮƽ�ƶ��Ƿ�ֹͣ���ı䷽��Ҳ��ֹͣ��

    private float currentLookAheadY;
    private float targetLookAheadY;
    private float smoothLookVelocityY;
    private float smoothVelocityY;

    //lastYΪ���ﾲֹʱ������ǰ����ֱλ��
    private float lastY;                
    private bool isLookAheadY;

    //�������С���λ��
    public Vector2 minPosition;
    public Vector2 maxPosition;
    // Start is called before the first frame update
    void Start()
    {
        // ��Start�г�ʼ��Ŀ�����
        target = GameObject.FindGameObjectWithTag("Player");
        // ����FocusArea�ĳ�ʼ��
        focusArea = new FocusArea(target.GetComponent<BoxCollider2D>().bounds, focusAreaSize);
    }

    private void LateUpdate()
    {
        // ����
        focusArea.Update(target.GetComponent<BoxCollider2D>().bounds);      

        // ƫ��
        Vector2 focusPosition = focusArea.center + offset;

        // ˮƽ����Ŀ���
        if (focusArea.velocity.x != 0)
        {
            // ��ȡˮƽ�����ٶȵķ��ţ��������������
            lookAheadDirX = Mathf.Sign(focusArea.velocity.x);

            // ��������뷽�����ƶ�����һ��ʱ������������ƶ���Ŀ�����
            if (Input.GetAxis("Horizontal") != 0 && Mathf.Sign(Input.GetAxis("Horizontal")) == Mathf.Sign(focusArea.velocity.x))
            {
                lookAheadStopped = false;
                targetLookAheadX = lookAheadDirX * lookAheadDstX;
            }
            else
            {
                // �����ֹͣ�����ı䷽��ʱ��������ƶ���Ŀ��λ����΢�ƶ�
                if (!lookAheadStopped)
                {
                    targetLookAheadX = currentLookAheadX + (lookAheadDirX * lookAheadDstX - currentLookAheadX) / 4;
                    lookAheadStopped = true;
                }
            }
        }
        //currentLookAheadX�ǵ�ǰ�ƶ��ľ��루ƽ�����ɣ���targetLookAheadXΪĿ���ƶ�����
        //SmoothDamp��һ���ƶ����ắ��������ʵ��ƽ���ƶ���Ч��
        currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);
        focusPosition += Vector2.right * currentLookAheadX;

        // ��ֱ����Ŀ���
        //target.CurrentVelocity.magnitude < 0.01f &&
        if (Input.GetAxis("Vertical") != 0)
        {
            if (!isLookAheadY)
            {
                //��¼�ƶ�ǰλ�ã����ڸ�λ
                lastY = transform.position.y;
                isLookAheadY = true;
            }
            //ʵ�����µ�������ƶ���ͬ��ʹ��SmoothDamp���ắ��
            targetLookAheadY = Input.GetAxis("Vertical") * lookAheadDstY;
            currentLookAheadY = Mathf.SmoothDamp(currentLookAheadY, targetLookAheadY, ref smoothLookVelocityY, lookSmoothTimeY);
            focusPosition += Vector2.up * currentLookAheadY;
        }
        else
        {
            // ������ƶ���ֹͣ������ֱ����ʱ���𽥸�λ��ֱƫ��
            if (isLookAheadY)
            {
                currentLookAheadY = 0;
                focusPosition.y = lastY;
                isLookAheadY = false;
            }
            focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);
        }

        // ���������λ��
        transform.position = (Vector3)focusPosition + Vector3.forward * -10;

        //�����������Χ
        Mathf.Clamp(transform.position.x, minPosition.x, maxPosition.x);
        Mathf.Clamp(transform.position.y, minPosition.y, maxPosition.y);
    }

    //����bounds�ı߿����
    void OnDrawGizmos()
    {
        focusArea.DrawBounds();
    }

    /// <summary>
    /// ���������������С����λ�ã������������Χ
    /// </summary>
    public void SetPosLimit(Vector2 minPos, Vector2 maxPos)
    {
        minPosition = minPos;
        maxPosition = maxPos;
    }
}

/// <summary>
/// ����һ���ṹ�壬������ʾ�����,���������������
/// </summary>
public struct FocusArea
{
    // ����
    public Vector2 center;
    // lΪ��ߣ�rΪ�ұߣ�tΪ������bΪ�ײ�
    float l, r, t, b;

    // �ٶ�
    public Vector2 velocity;

    public FocusArea(Bounds targetBounds, Vector2 focusAreaSize)
    {
        // ����center�ļ���
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
        // ��Ŀ�곬����߽�ʱ��������Ҫ�ƶ��ľ���
        if (targetBounds.min.x < l)
        {
            shiftX = targetBounds.min.x - l;
        }
        // ��Ŀ�곬���ұ߽�ʱ��������Ҫ�ƶ��ľ���
        else if (targetBounds.max.x > r)
        {
            shiftX = targetBounds.max.x - r;
        }

        l += shiftX;
        r += shiftX;

        float shiftY = 0;

        // ��Ŀ�곬���ײ��߽�ʱ��������Ҫ�ƶ��ľ���
        if (targetBounds.min.y < b)
        {
            shiftY = targetBounds.min.y - b;
        }
        // ��Ŀ�곬�������߽�ʱ��������Ҫ�ƶ��ľ���
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
    /// ��ʾbounds�ı߿�debugʹ��
    /// </summary>
    public void DrawBounds()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, new Vector3(r - l, t - b, 0));
    }

}
