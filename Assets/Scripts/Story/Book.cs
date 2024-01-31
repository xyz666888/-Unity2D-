using UnityEngine;

public class Book : MonoBehaviour {

    public Transform[] pageLeaf; // 页脚
    public AudioClip openCloseSound; // 打开关闭声音
    public AudioClip pageTurnSound; // 翻页声音
    public string SceneName; // 场景名称

    private Transform[] pageAnim; // 页动画
    private float[] pageAngle; // 页角度
    private float[] pageAngleMin; // 页最小角度
    private float[] pageAngleMax; // 页最大角度
    private float speed = 150.0f; // 速度
    private int page = -1; // 页数
    private int totalPages; // 总页数
    private AudioSource myAudio; // 音频源
    private float timer = 0; // 计时器


    /// <summary>
    /// 在对象被实例化时调用Start方法。
    /// 它会缓存音频源，设置页数，设置最小和最大角度，设置动画。
    /// </summary>
    void Start () {

        // 缓存音频源

        myAudio = GetComponent<AudioSource>();

        // 设置页数

        totalPages = pageLeaf.Length;
        pageAnim = new Transform[totalPages];
        pageAngle = new float[totalPages];
        pageAngleMin = new float[totalPages];
        pageAngleMax = new float[totalPages];

        // 遍历所有的页
        // 设置最小和最大角度，设置动画

        for (int i = 0 ; i < totalPages ; i++) {
			pageAngleMin[i] = pageLeaf[i].localEulerAngles.y;
			pageAngleMax[i] = pageLeaf[i].localEulerAngles.y + 170;
			pageAnim[i] = pageLeaf[i].Find("Page");
			if (pageAnim[i] != null) {
				pageAnim[i].GetComponent<Animation>()["RL"].speed = 2.0f;
				pageAnim[i].GetComponent<Animation>()["LR"].speed = 2.0f;
			}
		}

	}

    /// 在每一帧调用Update方法。
    /// 它会遍历所有的页，如果需要，会翻页并播放动画。
    /// </summary>
    void Update()
    {

        // 遍历所有的页
        // 如果需要，翻页并播放动画

        for (int i = 0 ; i < totalPages ; i++) {
			if (page >= i) {
				pageAngle[i] += Time.deltaTime * speed;
				if (pageAnim[i] != null)
					pageAnim[i].GetComponent<Animation>().Play("RL");
			} else {
				pageAngle[i] -= Time.deltaTime * speed;
				if (pageAnim[i] != null)
					pageAnim[i].GetComponent<Animation>().Play("LR");
			}
			pageAngle[i] = Mathf.Clamp(pageAngle[i], pageAngleMin[i], pageAngleMax[i]);
			pageLeaf[i].localEulerAngles = new Vector3(0.0f, pageAngle[i], 0.0f);
		}

		if (page == totalPages - 1)
		{
			timer += Time.deltaTime;
			if (timer >= 2)
			{
				if (SceneName == null || SceneName == "")
					return;
				
                UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
            }
		}

	}

    /// <summary>
    /// 翻页方法。
    /// 它会在可能的情况下按指定的方向翻页，并播放翻页和书本打开/关闭的声音效果。
    /// </summary>
    public void TurnPage(int direction)
    {

        // 在可能的情况下按指定的方向翻页
        // 播放翻页和书本打开/关闭的声音效果

        switch (direction) {
			case -1 :
				if (page < totalPages-1) {
					page++;
					if (page == 0 || page == totalPages-1)
						myAudio.PlayOneShot(openCloseSound);
					else
						myAudio.PlayOneShot(pageTurnSound);
				}
				break;
			case 1 :
				if (page > -1) {
					page--;
					if (page == -1 || page == totalPages-2)
						myAudio.PlayOneShot(openCloseSound);
					else
						myAudio.PlayOneShot(pageTurnSound);
				}
				break;
		}

	}


    void TurnToPage (int num) {


        page = num;
        myAudio.PlayOneShot(pageTurnSound);

    }

}