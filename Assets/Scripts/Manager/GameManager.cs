using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject rootObject; // 가장 상위 오브젝트
    public GameObject circle; // 원
    public GameObject judgeLine; // 판정 선
    public Text timeText; // 시간 텍스트
    public Text countDownText; // 카운트 다운 텍스트
    public string stageName; // 스테이지 이름
    public float currentTime = 0; // 지금 시간
    public float countDownTime = 2; // 카운트 다운 할 시간
    public float judgeLineY = 0f; // 판정 선의 Y 값

    public bool isGameStart = false;

    public bool isEditerMode = true; // 에디터인지 아닌지 bool 변수

    private float waitTimeTemp;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("GameManager가 여러개 생성되었습니다");
        }
        instance = this;

        waitTimeTemp = countDownTime;

        stageName = RecvManager.instance.stageName;
    }

    void Update()
    {
        if(!isEditerMode) // 에디터 모드가 아닐 경우 게임 스타트 버튼을 누르면 시작
        {
            if(InputManager.instance.isGameStartKeyDown)
            {
                GameStart();
            }
        }
        judgeLineY = Vector3.Distance(circle.transform.position, judgeLine.transform.position);

        if (!isGameStart) // 아직 시작 하지 않았을 경우 리턴
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
        if (isGameStart) yield break; // 이미 시작 했으면 break;

        NoteManager.instance.SetHavingNotes(); // 노트 셋팅

        countDownText.gameObject.SetActive(true); // 카운트 다운 시작

        while (waitTime >= 0) // 카운트 다운이 끝날때까지 반복
        {
            countDownText.text = Mathf.CeilToInt(waitTime).ToString();
            waitTime -= Time.deltaTime * 2;
            yield return null;
        }

        countDownText.gameObject.SetActive(false); // 끝났으니 비활성화

        // 시작
        isGameStart = true;
        AudioManager.instance.SetTimeAndStart();
        instance.currentTime = AudioManager.instance.musicCurrentTime;
        instance.timeText.text = "Time : " + instance.currentTime;
    }

    public void GameReset()
    {
        if (!isGameStart) return; // 이미 게임중이 아니라면 리턴

        isGameStart = false; // 게임중 아님

        if(!isEditerMode)
        {
            SceneManager.LoadScene("MusicSelect");
        }

        AudioManager.instance.MusicStop(); // 음악 멈추기
        NoteManager.instance.NotesReset(); // 노트 리셋
        
        if(ConvenienceManager.instance.SpawnNotesToggle != null)
        {
            ConvenienceManager.instance.SetStartingNotes(); // 에디터 모드에서 클릭 한 노트 셋팅
        }
        ComboManager.instance.ComboReset(); // 콤보 리셋

        instance.countDownTime = instance.waitTimeTemp; // 카운트 다운 리셋
        instance.currentTime = 0;
        instance.timeText.text = "Time : " + 0; // 시간 리셋
    }
}
