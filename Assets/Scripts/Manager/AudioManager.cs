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
    public AudioClip audioClip; // �÷��� �� ����
    public Text barCurrentTimeText; // ���൵ ���� �ð� �ؽ�Ʈ

    public Scrollbar scrollbar; // ���൵ ��

    public float musicLength = 0; // ���� ����

    public float musicCurrentTime = 0f; // ���� ���� ���൵

    private void Awake()
    {
        if(instance != null)        
        {
            Debug.LogError("�������� AudioManager�� �ֽ��ϴ�");
        }
        instance = this;

        audioSource.clip = audioClip;
        musicLength = audioSource.clip.length;
    }

    private void Update()
    {
        audioSource.volume = audioSoundPower; // ���� ũ�⿡ ���� ���� ����

        if(scrollbar != null) // ���� ó��
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
            audioSource.time = musicCurrentTime;
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
}
