using UnityEngine;
using System.Collections;
using System.Diagnostics.Tracing;
using UnityEngine.UI;

/// <summary>
/// ��ͣ�˵�����esc�������˵����ٴΰ��·�����Ϸ
/// ��ͣʱ��Ϸ������ˡ����־���ͣ
/// </summary>
public class Exit : MonoBehaviour
{

    public GameObject exitPanel = null; //���
    public GameObject musicPlayer;
    public Button cancel;      //ȡ����ť
    public Button quit;     //ȷ����ť
    public Slider volumeSlider;
    public AudioSource musicSource;

    private bool isStopped = false;

    // Use this for initialization
    void Start()
    {
        cancel.onClick.AddListener(Cancel);
        quit.onClick.AddListener(Quit);
        musicSource = musicPlayer.GetComponent<AudioSource>();
        // ����Slider�ĳ�ʼֵΪ���ֵ�����
        volumeSlider.value = musicSource.volume;
        // ���Slider��OnValueChanged�¼�����
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
        // ��Slider��ֵ�ı�ʱ����������������ı����ֵ�����
        musicSource.volume = volumeSlider.value;
    }

    //ȡ����
    private void Cancel()
    {
        exitPanel.SetActive(false);
        Time.timeScale = 1;
        isStopped = false;
        Debug.Log("this is cancel");
    }

    //ȷ���˳�
    private void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// �ж��Ƿ񵯳��˵�
    /// </summary>
    IEnumerator  PopMenu()
    {
        if (!isStopped)
        {
            exitPanel.SetActive(true);
            yield return null;

            //�������ɼ�����ͣ
            //Cursor.visible = true;
            isStopped = true;
            Time.timeScale = 0;
        }
        else
        {
            exitPanel.SetActive(false);
            yield return null;

            //���������ʧ������
            //Cursor.visible = false;
            isStopped = false;
            Time.timeScale = 1;
        }
        yield break;
    }

    /// <summary>
    /// �������ֵ���ͣ�벥��
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