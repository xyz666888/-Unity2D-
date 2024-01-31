using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItem : MonoBehaviour
{
    public bool isPicked;
    public float destroyDelay = 10f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyProp", destroyDelay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
