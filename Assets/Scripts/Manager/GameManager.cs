using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject rootObject;
    public GameObject circle;
    public GameObject judgeLine;
    public Text timeText;
    public Text countDownText;
    public float currentTime = 0;
    public float countDownTime = 2;
    public float judgeLineY = 0f;

    public bool isGameStart = false;

    private float waitTimeTemp;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("GameManager가 여러개 생성되었습니다");
        }
        instance = this;

        waitTimeTemp = countDownTime;
    }

    void Update()
    {
        judgeLineY = Vector3.Distance(circle.transform.position, judgeLine.transform.position);

        if (!isGameStart)
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

    // Start 버튼을 누른 뒤 카운트 다운을 하고
    // 이후 시작하는 함수를 실행하는 코루틴
    private IEnumerator GameStarting(float waitTime)
    {
        if (isGameStart) yield break;

        NoteManager.instance.SetHavingNotes();

        countDownText.gameObject.SetActive(true);

        while (waitTime >= 0)
        {
            countDownText.text = Mathf.FloorToInt(waitTime).ToString();
            waitTime -= Time.deltaTime * 2;
            yield return null;
        }

        countDownText.gameObject.SetActive(false);

        isGameStart = true;
        AudioManager.instance.SetTimeAndStart();
        instance.currentTime = AudioManager.instance.musicCurrentTime;
        instance.timeText.text = "Time : " + instance.currentTime;
    }

    public void GameReset()
    {
        if (isGameStart == false) return;

        isGameStart = false;

        AudioManager.instance.MusicStop();
        NoteManager.instance.NotesReset();
        ConvenienceManager.instance.SetStartingNotes();
        ComboManager.instance.ComboReset();

        instance.countDownTime = instance.waitTimeTemp;
        instance.currentTime = 0;
        instance.timeText.text = "Time : " + 0;
    }
}
