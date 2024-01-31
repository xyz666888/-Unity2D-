using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeadPanel : MonoBehaviour
{
    public int deadTimes = 0;
    public GameObject deadTimesPanel;
    public TextMeshProUGUI deadText;
    public Button reviveButton;
    public bool isDead = false;
    void Start()
    {
        //复活按钮
        reviveButton.onClick.RemoveAllListeners();
        reviveButton.onClick.AddListener(Revive);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerAttribute.Instance.currentBlood <= 0)
        {
            isDead = true;
            StartCoroutine("DeadTimes");
        }
        else
        {
            StopCoroutine("DeadTimes");
        }
    }

    IEnumerator  DeadTimes()
    {
        if (isDead)
        {
            yield return new WaitForSeconds(2);
            deadTimesPanel.SetActive(true);
            //修改面板文字内容
            if (deadTimes == 0)
            {
                deadText.text = "你死了";
            }
            if (deadTimes == 1)
            {
                deadText.text = "你又死了";
            }
            else if (deadTimes == 2)
            {
                deadText.text = "你双死了";
            }
            else if (deadTimes == 3)
            {
                deadText.text = "你叒死了";
            }
            else if(deadTimes >= 4)
            {
                deadText.text = "你叕死了";
            }
            deadTimes++;
            Time.timeScale = 0;
            isDead = false;
            PlayerAttribute.Instance.currentBlood = 100;
        }
    }

    private void Revive()
    {
        //复活
        Time.timeScale = 1;
        deadTimesPanel.SetActive(false);
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            PlayerAttribute.Instance.gameObject.transform.position = new Vector3(-4, 35, 0);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            PlayerAttribute.Instance.gameObject.transform.position = new Vector3(-11, -4, 0);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            PlayerAttribute.Instance.gameObject.transform.position = new Vector3(-10, -11, 0);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 6)
        {
            PlayerAttribute.Instance.gameObject.transform.position = new Vector3(367, 1, 0);
        }

        PlayerAttribute.Instance.GetComponent<Animator>().SetTrigger("Revive");
    }
}
