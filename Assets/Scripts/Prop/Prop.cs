using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Prop : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }


    /**
     * 道具拾取效果方法
     * 拾取时调用
     * 拾取该道具后产生的效果
     * 子类中需要根据道具不同覆写的方法
     */
    public virtual void PickedEffect() { }

}
