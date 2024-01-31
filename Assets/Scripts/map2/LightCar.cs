using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCar : MonoBehaviour
{
    public GameObject light;
    public SpriteRenderer sp;
    public bool isLight;
    public 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Light(light));
    }

    IEnumerator Light(GameObject light)
    {
        LightUp lightup = light.GetComponent<LightUp>();
      
        if (lightup.num >= 1)
        {
            sp = GetComponent<SpriteRenderer>();
            sp.color = new UnityEngine.Color(sp.color.r, sp.color.g, sp.color.b, 1);
            isLight = true;
        }
        yield return null;
    }
}
