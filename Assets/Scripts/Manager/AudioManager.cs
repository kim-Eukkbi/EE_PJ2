using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSource;
    public AudioClip audioClip;

    public Scrollbar scrollbar;

    public float musicLength = 0;

    public float musicCurrentTime = 0f;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("여러개의 AudioManager가 있습니다");
        }
        instance = this;

        audioSource.clip = audioClip;
        musicLength = audioSource.clip.length;
    }

    private void Update()
    {
        musicCurrentTime = musicLength * scrollbar.value;
    }

    public void SetTimeAndStart()
    {
        audioSource.time = musicCurrentTime;
        audioSource.Play();
    }

    public void MusicStop()
    {
        audioSource.Stop();
    }

    public void SetScrollBar()
    {
        if(GameManager.instance.currentTime / musicLength >= 1f)
        {
            GameManager.instance.GameReset();
        }

        scrollbar.value = GameManager.instance.currentTime / musicLength;
    }
}
