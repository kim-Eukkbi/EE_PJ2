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
    public float currentTime = 0;
    public float waitTime = 2;
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

        waitTimeTemp = waitTime;
    }

    void Update()
    {
        if (!isGameStart)
        {
            instance.waitTime -= Time.deltaTime;
            return;
        }

        currentTime += Time.deltaTime;
        timeText.text = "Time : " + currentTime.ToString("0.000");
        AudioManager.instance.SetScrollBar();

        judgeLineY = Vector3.Distance(circle.transform.position, judgeLine.transform.position);
    }

    public void GameStart()
    {
        StartCoroutine(GameStarting(waitTime));
    }

    private IEnumerator GameStarting(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        isGameStart = true;
        AudioManager.instance.SetTimeAndStart();
        instance.currentTime = AudioManager.instance.musicCurrentTime;
        instance.timeText.text = "Time : " + instance.currentTime;

        NoteManager.instance.SetNoteTimePosition();
    }

    public void GameReset()
    {
        isGameStart = false;

        NoteManager.instance.NotesReset();
        ComboManager.instance.ComboReset();
        AudioManager.instance.MusicStop();

        instance.waitTime = instance.waitTimeTemp;
        instance.currentTime = 0;
        instance.timeText.text = "Time : " + 0;
    }
}
