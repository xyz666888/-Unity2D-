using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gear : Prop
{
    private Animator animator;
    private GearDoor gearDoor;
    private bool isopened;
    [Header("�󶨵Ļ�����GameObject")]
    public GameObject gearDoorPrefab;


    // Start is called before the first frame update
    void Start()
    {
        gearDoor = gearDoorPrefab.GetComponent<GearDoor>();
        animator = GetComponent<Animator>();
        isopened = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /**
     * ���ŷ���
     * ��ײ�����Ұ�Fʱ�����õķ���
     */
    public override void PickedEffect()
    {
        if (!isopened)
        {
            //���ſ���״̬ת������
            animator.SetTrigger("openning");
            isopened = true;
            gearDoor.OpenGearDoor();
        }
    }

}
