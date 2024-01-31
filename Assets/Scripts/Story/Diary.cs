using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Diary : Prop
{
    public override void PickedEffect()
    {
        SceneManager.LoadScene("Postscript");
    }
}
