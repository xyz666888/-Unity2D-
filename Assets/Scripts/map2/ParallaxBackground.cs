using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    //序列化一个Vector2向量，用来区分每个背景图层的视差系数
    [SerializeField] private Vector2 parallaxEffectMultiplier;
    private Camera mainCamera;
    private Transform cameraTransform;
    private Vector3 lastCameraPosition; //摄像机上一帧位置
    public float mapWidth; //地图宽度
    public int mapNums; //地图重复的次数

    private float totalWidth; //总地图宽度
    private void Start()
    {
        mainCamera = Camera.main;
        cameraTransform = mainCamera.transform;
        lastCameraPosition = cameraTransform.position;  //记录当前摄像机位置
        mapWidth = GetComponent<SpriteRenderer>().sprite.bounds.size.x; //获得图像宽度
        totalWidth = mapWidth * mapNums;    //总宽度
    }

    private void Update()
    {
        InfMap();
    }
    private void LateUpdate()
    {
        MapMove();
    }

    /// <summary>
    /// 该函数实现了地图对摄像机的视差跟随
    /// 包括对水平方向的视差与垂直方向的视差
    /// </summary>
    private void MapMove()
    {
        //deltamovement是摄像机移动的距离
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        //地图移动距离即加上deltamovement与视差系数的乘积
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x,
                                            deltaMovement.y * parallaxEffectMultiplier.y);
        lastCameraPosition = cameraTransform.position;
    }

    /// <summary>
    /// 该函数实现了无限地图的功能，原理即三个地图的交替平移
    /// </summary>
   private void InfMap()
    {
        Vector3 tempPosition = transform.position;  //获取当前位置
        if(cameraTransform.position.x > transform.position.x + totalWidth / 2)
        {
            tempPosition.x += totalWidth;   //地图向右平移一个完整地图宽度
            transform.position = tempPosition;  //更新位置
        }
        if (cameraTransform.position.x < transform.position.x - totalWidth / 2)
        {
            tempPosition.x -= totalWidth;   //地图向左平移一个完整地图宽度
            transform.position = tempPosition;  //更新位置
        }
    }
}
