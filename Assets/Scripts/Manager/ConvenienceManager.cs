using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConvenienceManager : MonoBehaviour
{
    public static ConvenienceManager instance;

    public LayerMask whatIsDCNote; // DCNote 의 layer
    public LayerMask whatIsSingleNote;// SingleNote 의 layer
    public LayerMask whatIsLongNote;// LongNote 의 layer

    public Toggle SpawnNotesToggle; // 플레이 도중 a, s, d 키를 통해서 노트를 생성 (true, false)

    private float circleAngle; // 원의 각도 
    private List<Note> startingNotes = new List<Note>(); // start 를 통해 시작 할때 잠시 임시로 노트들을 저장

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("여러개의 ConvenienceManager가 생성되었습니다");
        }
        instance = this;
    }
    void Update()
    {
        circleAngle = GameManager.instance.circle.transform.rotation.eulerAngles.z;

        if (circleAngle < 0) // 예외 처리 구문
        {
            circleAngle += 360;
        }

        if (!GameManager.instance.isGameStart) // 게임 에디터 모드에서 플레이 도중이 아닐 때
        {
            if (InputManager.instance.SingleNoteSpawnKeyDown)
            {
                SpawnNoteMousePos(NoteManager.NoteEnum.Single);
            }
            else if(InputManager.instance.DCNoteSpawnKeyDown)
            {
                SpawnNoteMousePos(NoteManager.NoteEnum.DC);
            }
            else if (InputManager.instance.LongNoteSpawnKeyDown)
            {
                SpawnNoteMousePos(NoteManager.NoteEnum.Long);
            }

            // 삭제 키를 누르면 마우스 위치에 있는 노트를 삭제 한다
            if(InputManager.instance.RemoveNoteKeyDown) 
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D ray = Physics2D.Raycast(mousePos, Camera.main.transform.forward * -1, 20, whatIsDCNote | whatIsSingleNote | whatIsLongNote);

                if (ray.collider != null)
                {   
                    NoteManager.instance.RemoveNote(ray.transform.GetComponent<Note>());
                }
            }
        }
        else // 에디터 모드에서 게임 플레이 상태 일때
        {
            if(SpawnNotesToggle.isOn)
            {
                // 노트 스폰 키를 아무거나 눌렀을 때 해당하는 노트를 생성하고 그에 대한 값을 넣은 뒤 잠시 비활성화
                if(InputManager.instance.DCNoteSpawnKeyDown ||
                    InputManager.instance.SingleNoteSpawnKeyDown ||
                    InputManager.instance.LongNoteSpawnKeyDown)
                {
                    Note note = null;

                    if (InputManager.instance.SingleNoteSpawnKeyDown)
                    {
                        note = PoolManager.GetCreateItem<SingleNote>(NoteManager.instance.singleNotePrefab);
                    }
                    else if(InputManager.instance.DCNoteSpawnKeyDown)
                    {
                        note = PoolManager.GetCreateItem<DCNote>(NoteManager.instance.dcNotePrefab);
                    }
                    else if (InputManager.instance.LongNoteSpawnKeyDown)
                    {
                        note = PoolManager.GetCreateItem<LongNote>(NoteManager.instance.longNotePrefab);
                    }

                    note.gameObject.SetActive(false);

                    note.time = GameManager.instance.currentTime;
                    note.angle = circleAngle;

                    startingNotes.Add(note);
                }
            }
        }
    }

    // stop을 할 시 노트를 다시 셋팅
    public void SetStartingNotes()
    {
        for(int i = 0; i < startingNotes.Count; i++)
        {
            NoteManager.instance.AddNote(startingNotes[i]);
            NoteManager.instance.SetNoteValue(startingNotes[i]);
        }

        startingNotes.Clear();
    }

    // 마우스 위치에 매개변수에 맞는 노트 생성
    private void SpawnNoteMousePos(NoteManager.NoteEnum noteEnum)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        float dis = Vector3.Distance(mousePos, GameManager.instance.circle.transform.position)
                                    - GameManager.instance.judgeLineY
                                    - GameManager.instance.judgeLine.transform.localScale.y;

        OptionManager.instance.GetCreateNote(circleAngle,
                                    AudioManager.instance.musicLength * AudioManager.instance.scrollbar.value + (dis / NoteManager.instance.noteSpeed),
                                    noteEnum);
        OptionManager.instance.UISetActive(2);
    }
}
