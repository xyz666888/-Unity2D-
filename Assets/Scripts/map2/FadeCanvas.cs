using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;
public class FadeCanvas : MonoBehaviour
{
    public Image fade;
    public bool isJump = false;
    // Start is called before the first frame update
    private static FadeCanvas instance;

    public static FadeCanvas Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(FadeCanvas)) as FadeCanvas;

                //Лђеп instance=new GameObject("[Singleton]").AddComponent<Singleton>();
            }

            return instance;
        }
    }

    private void Update()
    {
        if(isJump)
        {
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1);
        fade.DOFade(0,1);
        isJump = false;
    }
}
