using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gear : Prop
{
    private Animator animator;
    private GearDoor gearDoor;
    private bool isopened;
    [Header("绑定的机关门GameObject")]
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
     * 开门方法
     * 碰撞触发且按F时，调用的方法
     */
    public override void PickedEffect()
    {
        if (!isopened)
        {
            //播放开关状态转换动画
            animator.SetTrigger("openning");
            isopened = true;
            gearDoor.OpenGearDoor();
        }
    }

}
