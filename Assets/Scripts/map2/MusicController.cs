using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public  AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayMusic()
    {
        audioSource.Play();
        Debug.Log("music is playing");
    }
}
