using UnityEngine;
using System.Collections;
using System.Diagnostics.Tracing;
using UnityEngine.UI;

/// <summary>
/// 暂停菜单，按esc键弹出菜单，再次按下返回游戏
/// 暂停时游戏人物、敌人、音乐均暂停
/// </summary>
public class Exit : MonoBehaviour
{

    public GameObject exitPanel = null; //面板
    public GameObject musicPlayer;
    public Button cancel;      //取消按钮
    public Button quit;     //确定按钮
    public Slider volumeSlider;
    public AudioSource musicSource;

    private bool isStopped = false;

    // Use this for initialization
    void Start()
    {
        cancel.onClick.AddListener(Cancel);
        quit.onClick.AddListener(Quit);
        musicSource = musicPlayer.GetComponent<AudioSource>();
        // 设置Slider的初始值为音乐的音量
        volumeSlider.value = musicSource.volume;
        // 添加Slider的OnValueChanged事件监听
        volumeSlider.onValueChanged.AddListener(delegate { SetVolume(); });
        
        DontDestroyOnLoad(this.exitPanel);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(PopMenu());
            StopMusic();
        }
    }

    public void SetVolume()
    {
        // 当Slider的值改变时，调用这个方法来改变音乐的音量
        musicSource.volume = volumeSlider.value;
    }

    //取消键
    private void Cancel()
    {
        exitPanel.SetActive(false);
        Time.timeScale = 1;
        isStopped = false;
        Debug.Log("this is cancel");
    }

    //确认退出
    private void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// 判断是否弹出菜单
    /// </summary>
    IEnumerator  PopMenu()
    {
        if (!isStopped)
        {
            exitPanel.SetActive(true);
            yield return null;

            //设置鼠标可见并暂停
            //Cursor.visible = true;
            isStopped = true;
            Time.timeScale = 0;
        }
        else
        {
            exitPanel.SetActive(false);
            yield return null;

            //设置鼠标消失并继续
            //Cursor.visible = false;
            isStopped = false;
            Time.timeScale = 1;
        }
        yield break;
    }

    /// <summary>
    /// 控制音乐的暂停与播放
    /// </summary>
    private void StopMusic()
    {
        if (!isStopped)
        {
            musicPlayer.GetComponent<AudioSource>().Pause();    
        }
        else
        {
            musicPlayer.GetComponent<AudioSource>().UnPause();
        }
    }
}