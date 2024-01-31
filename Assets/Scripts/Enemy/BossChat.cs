using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChat : MonoBehaviour
{
    public string chatName;

    private Flowchart flowchart;
    private bool isTalked = false;
    //是否可以对话

    // Start is called before the first frame update
    void Start()
    {
        flowchart = GameObject.Find("Flowchart").GetComponent<Flowchart>();

    }

    // Update is called once per frame
    void Update()
    {
        Say();
    }

    private void Say()
    {

        if (GetComponent<Boss>().health <= 0 && !isTalked)
        {
            if (flowchart.HasBlock(chatName))
            {
                flowchart.ExecuteBlock(chatName);
                isTalked = true;
            }
        }
    }
}
