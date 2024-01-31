using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class LoginClass : MonoBehaviour
{
    //进入前变量
    public TMP_InputField username, password, confirmPassword;
    public TMP_Text reminderText;
    public int errorsNum;
    public Button loginButton;
    public GameObject hallSetUI, loginUI;
    //进入后变量
    public static string myUsername;

    //注册板块
    //存储简单的键值对数据
    //并根据用户输入给出相应的反馈
    public void Register()
    {
        //并无该用户存在
        //进行注册
        if (PlayerPrefs.GetString(username.text) == "")
        {
            //保证注册时输入两次密码相等
            if (password.text == confirmPassword.text)
            {
                //存储简单的键值对数据
                PlayerPrefs.SetString(username.text, username.text);
                PlayerPrefs.SetString(username.text + "password", password.text);
                reminderText.text = "注册成功！";
            }
            else//反馈
            {
                reminderText.text = "两次密码输入不一致";
            }
        }
        else//反馈
        {
            reminderText.text = "用户已存在";
        }

    }
    private void Recovery()
    {
        loginButton.interactable = true;
    }

    //登录板块
    public void Login()
    {
        //该用户已存在
        if (PlayerPrefs.GetString(username.text) != "")
        {
            if (PlayerPrefs.GetString(username.text + "password") == password.text)
            {
                reminderText.text = "登录成功";

                myUsername = username.text;
                hallSetUI.SetActive(true);
                loginUI.SetActive(false);
                SceneManager.LoadScene("Preface");
            }
            else
            {
                reminderText.text = "密码错误";
                errorsNum++;
                if (errorsNum >= 3)
                {
                    reminderText.text = "连续错误3次，请30秒后再试！";
                    loginButton.interactable = false;
                    Invoke("Recovery", 5);
                    errorsNum = 0;
                }
            }
        }
        else
        {
            reminderText.text = "账号不存在";
        }
    }
}

