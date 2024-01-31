using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    //���л�һ��Vector2��������������ÿ������ͼ����Ӳ�ϵ��
    [SerializeField] private Vector2 parallaxEffectMultiplier;
    private Camera mainCamera;
    private Transform cameraTransform;
    private Vector3 lastCameraPosition; //�������һ֡λ��
    public float mapWidth; //��ͼ���
    public int mapNums; //��ͼ�ظ��Ĵ���

    private float totalWidth; //�ܵ�ͼ���
    private void Start()
    {
        mainCamera = Camera.main;
        cameraTransform = mainCamera.transform;
        lastCameraPosition = cameraTransform.position;  //��¼��ǰ�����λ��
        mapWidth = GetComponent<SpriteRenderer>().sprite.bounds.size.x; //���ͼ����
        totalWidth = mapWidth * mapNums;    //�ܿ��
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
    /// �ú���ʵ���˵�ͼ����������Ӳ����
    /// ������ˮƽ������Ӳ��봹ֱ������Ӳ�
    /// </summary>
    private void MapMove()
    {
        //deltamovement��������ƶ��ľ���
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        //��ͼ�ƶ����뼴����deltamovement���Ӳ�ϵ���ĳ˻�
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x,
                                            deltaMovement.y * parallaxEffectMultiplier.y);
        lastCameraPosition = cameraTransform.position;
    }

    /// <summary>
    /// �ú���ʵ�������޵�ͼ�Ĺ��ܣ�ԭ��������ͼ�Ľ���ƽ��
    /// </summary>
   private void InfMap()
    {
        Vector3 tempPosition = transform.position;  //��ȡ��ǰλ��
        if(cameraTransform.position.x > transform.position.x + totalWidth / 2)
        {
            tempPosition.x += totalWidth;   //��ͼ����ƽ��һ��������ͼ���
            transform.position = tempPosition;  //����λ��
        }
        if (cameraTransform.position.x < transform.position.x - totalWidth / 2)
        {
            tempPosition.x -= totalWidth;   //��ͼ����ƽ��һ��������ͼ���
            transform.position = tempPosition;  //����λ��
        }
    }
}
