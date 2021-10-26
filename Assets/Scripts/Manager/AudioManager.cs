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
        if(scrollbar != null)
        {
            musicCurrentTime = musicLength * scrollbar.value;
        }
    }

    // 지금 시간에 음악을 시작
    public void SetTimeAndStart()
    {
        audioSource.time = musicCurrentTime;
        audioSource.Play();
    }

    // 음악 멈춰
    public void MusicStop()
    {
        audioSource.Stop();
    }

    // 게임 진행 도중 음악이 끝이 나면 게임 리셋,
    // 이외에는 스크롤 바를 음악 진행속도에 맞춰 이동
    public void SetScrollBar()
    {
        if(GameManager.instance.currentTime / musicLength >= 1f)
        {
            GameManager.instance.GameReset();
            return;
        }

        scrollbar.value = GameManager.instance.currentTime / musicLength;
    }
}
