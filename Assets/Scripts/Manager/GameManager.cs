using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject rootObject; // ���� ���� ������Ʈ
    public GameObject circle; // ��
    public GameObject judgeLine; // ���� ��
    public Text timeText; // �ð� �ؽ�Ʈ
    public Text countDownText; // ī��Ʈ �ٿ� �ؽ�Ʈ
    public string stageName; // �������� �̸�
    public float currentTime = 0; // ���� �ð�
    public float countDownTime = 2; // ī��Ʈ �ٿ� �� �ð�
    public float judgeLineY = 0f; // ���� ���� Y ��

    public bool isGameStart = false;

    public bool isEditerMode = true; // ���������� �ƴ��� bool ����

    private float waitTimeTemp;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("GameManager�� ������ �����Ǿ����ϴ�");
        }
        instance = this;

        waitTimeTemp = countDownTime;

        stageName = RecvManager.instance.stageName;
    }

    void Update()
    {
        if(!isEditerMode) // ������ ��尡 �ƴ� ��� ���� ��ŸƮ ��ư�� ������ ����
        {
            if(InputManager.instance.isGameStartKeyDown)
            {
                GameStart();
            }
        }
        judgeLineY = Vector3.Distance(circle.transform.position, judgeLine.transform.position);

        if (!isGameStart) // ���� ���� ���� �ʾ��� ��� ����
        {
            return;
        }

        currentTime += Time.deltaTime;
        timeText.text = "Time : " + currentTime.ToString("0.000");
        AudioManager.instance.SetScrollBar();
    }

    public void GameStart()
    {
        StartCoroutine(GameStarting(countDownTime));
    }

    // Start ��ư�� ���� �� ī��Ʈ �ٿ��� �ϰ�
    // ���� �����ϴ� �Լ��� �����ϴ� �ڷ�ƾ
    private IEnumerator GameStarting(float waitTime)
    {
        if (isGameStart) yield break; // �̹� ���� ������ break;

        NoteManager.instance.SetHavingNotes(); // ��Ʈ ����

        countDownText.gameObject.SetActive(true); // ī��Ʈ �ٿ� ����

        while (waitTime >= 0) // ī��Ʈ �ٿ��� ���������� �ݺ�
        {
            countDownText.text = Mathf.CeilToInt(waitTime).ToString();
            waitTime -= Time.deltaTime * 2;
            yield return null;
        }

        countDownText.gameObject.SetActive(false); // �������� ��Ȱ��ȭ

        // ����
        isGameStart = true;
        AudioManager.instance.SetTimeAndStart();
        instance.currentTime = AudioManager.instance.musicCurrentTime;
        instance.timeText.text = "Time : " + instance.currentTime;
    }

    public void GameReset()
    {
        if (!isGameStart) return; // �̹� �������� �ƴ϶�� ����

        isGameStart = false; // ������ �ƴ�

        if(!isEditerMode)
        {
            SceneManager.LoadScene("MusicSelect");
        }

        AudioManager.instance.MusicStop(); // ���� ���߱�
        NoteManager.instance.NotesReset(); // ��Ʈ ����
        
        if(ConvenienceManager.instance.SpawnNotesToggle != null)
        {
            ConvenienceManager.instance.SetStartingNotes(); // ������ ��忡�� Ŭ�� �� ��Ʈ ����
        }
        ComboManager.instance.ComboReset(); // �޺� ����

        instance.countDownTime = instance.waitTimeTemp; // ī��Ʈ �ٿ� ����
        instance.currentTime = 0;
        instance.timeText.text = "Time : " + 0; // �ð� ����
    }
}
