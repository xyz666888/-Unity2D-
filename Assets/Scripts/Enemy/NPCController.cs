using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public string chatName;
    public string chatName_;

    private Flowchart flowchart;
    private bool isTalked_1 = false;
    private bool isTalked_2 = false;
    //�Ƿ���ԶԻ�
    
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

    private bool EstimateDistance()
    {
        float distance = transform.position.x - PlayerAttribute.Instance.transform.position.x;
        if (distance < 15f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Say()
    {
        if(EstimateDistance() && !isTalked_1)
        {
            //�Ի��Ƿ����
            if (flowchart.HasBlock(chatName))
            {
                flowchart.ExecuteBlock(chatName);
                isTalked_1 = true;
            }
        }

        if(GetComponent<Monster>().blood <= 0 && !isTalked_2) 
        {
            if (flowchart.HasBlock(chatName_))
            {
                flowchart.ExecuteBlock(chatName_);
                isTalked_2 = true;
            }
        }
    }
}
