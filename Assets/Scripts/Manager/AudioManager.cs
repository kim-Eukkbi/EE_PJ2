using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSource; // �÷��� �� ������Ʈ
    [Range(0, 1)]
    public float audioSoundPower; // ���� �Ҹ��� ũ��
    public Scrollbar audioPitchBar;
    public Text pitchText;
    public AudioClip audioClip; // �÷��� �� ����
    public Text barCurrentTimeText; // ���൵ ���� �ð� �ؽ�Ʈ

    public Scrollbar scrollbar; // ���൵ ��

    public float musicLength = 0; // ���� ����

    public float musicCurrentTime = 0f; // ���� ���� ���൵

    public float audioPitch = 0;

    private void Awake()
    {
        if(instance != null)        
        {
            Debug.LogError("�������� AudioManager�� �ֽ��ϴ�");
        }
        instance = this;

        audioSource.clip = audioClip;
        audioPitch = Mathf.Clamp(audioPitchBar.value * 3, 0.2f, 3);
        musicLength = audioSource.clip.length / audioPitch;


        // ��� �׽�Ʈ ------------------------------------------------------------------------------------------------------------------------------
    }

    private void Update()
    {
        audioSource.volume = audioSoundPower; // ���� ũ�⿡ ���� ���� ����

        SetPitch();
        musicLength = audioSource.clip.length / audioPitch;

        if (scrollbar != null) // ���� ó��
        {
            musicCurrentTime = musicLength * scrollbar.value; // ������ ���� ���൵ �ٸ� �����δ�

            barCurrentTimeText.text = (musicLength * scrollbar.value).ToString("0.0"); // ���൵ �� �ؽ�Ʈ�� �׿� ���� �Ҵ�
        }
    }

    // ���� �ð��� ������ ����
    public void SetTimeAndStart()
    {
        if(musicCurrentTime < musicLength)
        {
            audioSource.time = musicCurrentTime * audioPitch;
            audioSource.Play();
        }
    }

    // ���� ����
    public void MusicStop()
    {
        audioSource.Stop();
    }

    // ���� ���� ���� ������ ���� ���� ���� ����,
    // �̿ܿ��� ��ũ�� �ٸ� ���� ����ӵ��� ���� �̵�
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
