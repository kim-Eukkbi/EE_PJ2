using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSource; // 플레이 할 오브젝트
    [Range(0, 1)]
    public float audioSoundPower; // 음악 소리의 크기
    public Scrollbar audioPitchBar;
    public Text pitchText;
    public AudioClip audioClip; // 플레이 할 음악
    public Text barCurrentTimeText; // 진행도 바의 시간 텍스트

    public Scrollbar scrollbar; // 진행도 바

    public float musicLength = 0; // 음악 길이

    public float musicCurrentTime = 0f; // 지금 음악 진행도

    public float audioPitch = 0;

    private void Awake()
    {
        if(instance != null)        
        {
            Debug.LogError("여러개의 AudioManager가 있습니다");
        }
        instance = this;

        audioSource.clip = audioClip;
        audioPitch = Mathf.Clamp(audioPitchBar.value * 3, 0.2f, 3);
        musicLength = audioSource.clip.length / audioPitch;


        // 잠시 테스트 ------------------------------------------------------------------------------------------------------------------------------
    }

    private void Update()
    {
        audioSource.volume = audioSoundPower; // 음악 크기에 따라 볼륨 조절

        SetPitch();
        musicLength = audioSource.clip.length / audioPitch;

        if (scrollbar != null) // 예외 처리
        {
            musicCurrentTime = musicLength * scrollbar.value; // 비율에 따라서 진행도 바를 움직인다

            barCurrentTimeText.text = (musicLength * scrollbar.value).ToString("0.0"); // 진행도 바 텍스트도 그에 따라 할당
        }
    }

    // 지금 시간에 음악을 시작
    public void SetTimeAndStart()
    {
        if(musicCurrentTime < musicLength)
        {
            audioSource.time = musicCurrentTime * audioPitch;
            audioSource.Play();
        }
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

    private void SetPitch()
    {
        audioPitch = Mathf.Clamp(audioPitchBar.value * 3, 0.2f, 3);
        audioSource.pitch = audioPitch;
        pitchText.text = audioPitch.ToString("0.000000");
    }
}
