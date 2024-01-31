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
        //���ť
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
            //�޸������������
            if (deadTimes == 0)
            {
                deadText.text = "������";
            }
            if (deadTimes == 1)
            {
                deadText.text = "��������";
            }
            else if (deadTimes == 2)
            {
                deadText.text = "��˫����";
            }
            else if (deadTimes == 3)
            {
                deadText.text = "�ㅪ����";
            }
            else if(deadTimes >= 4)
            {
                deadText.text = "�ㅬ����";
            }
            deadTimes++;
            Time.timeScale = 0;
            isDead = false;
            PlayerAttribute.Instance.currentBlood = 100;
        }
    }

    private void Revive()
    {
        //����
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
