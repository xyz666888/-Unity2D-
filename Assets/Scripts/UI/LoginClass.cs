using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class LoginClass : MonoBehaviour
{
    //����ǰ����
    public TMP_InputField username, password, confirmPassword;
    public TMP_Text reminderText;
    public int errorsNum;
    public Button loginButton;
    public GameObject hallSetUI, loginUI;
    //��������
    public static string myUsername;

    //ע����
    //�洢�򵥵ļ�ֵ������
    //�������û����������Ӧ�ķ���
    public void Register()
    {
        //���޸��û�����
        //����ע��
        if (PlayerPrefs.GetString(username.text) == "")
        {
            //��֤ע��ʱ���������������
            if (password.text == confirmPassword.text)
            {
                //�洢�򵥵ļ�ֵ������
                PlayerPrefs.SetString(username.text, username.text);
                PlayerPrefs.SetString(username.text + "password", password.text);
                reminderText.text = "ע��ɹ���";
            }
            else//����
            {
                reminderText.text = "�����������벻һ��";
            }
        }
        else//����
        {
            reminderText.text = "�û��Ѵ���";
        }

    }
    private void Recovery()
    {
        loginButton.interactable = true;
    }

    //��¼���
    public void Login()
    {
        //���û��Ѵ���
        if (PlayerPrefs.GetString(username.text) != "")
        {
            if (PlayerPrefs.GetString(username.text + "password") == password.text)
            {
                reminderText.text = "��¼�ɹ�";

                myUsername = username.text;
                hallSetUI.SetActive(true);
                loginUI.SetActive(false);
                SceneManager.LoadScene("Preface");
            }
            else
            {
                reminderText.text = "�������";
                errorsNum++;
                if (errorsNum >= 3)
                {
                    reminderText.text = "��������3�Σ���30������ԣ�";
                    loginButton.interactable = false;
                    Invoke("Recovery", 5);
                    errorsNum = 0;
                }
            }
        }
        else
        {
            reminderText.text = "�˺Ų�����";
        }
    }
}

