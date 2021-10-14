using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public static NoteManager instance;

    public GameObject dcNotePrefab;
    public GameObject singleNotePrefab;
    public GameObject longNotePrefab;
    public Transform parent;

    public float noteSpeed = 1;
    public float noteLength = 5;

    private List<Note> notes;
    private int count = 0;

    /// <summary>
    /// DC == DontClick,
    /// Single == One Click, 
    /// Long == Long Note
    /// </summary>
    public enum NoteEnum
    {
        DC,
        Single,
        Long
    }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("NoteManager가 여러개 생성되었습니다");
        }
        instance = this;

        notes = new List<Note>();
    }

    private void Start()
    {
        // 각각의 노트들의 풀 만들기
        PoolManager.CreatePool<DCNote>(dcNotePrefab, parent, 50);
        PoolManager.CreatePool<SingleNote>(singleNotePrefab, parent, 50);
        PoolManager.CreatePool<LongNote>(longNotePrefab, parent, 50);

        SetNotePattern();

        notes.Sort((x, y) => x.time.CompareTo(y.time));

        //for (int i = 0; i < notes.Count; i++)
        //{
        //    Debug.Log(notes[i].time);
        //}
    }

    private void Update()
    {
        if (GameManager.instance.currentTime < 2f) return; // 시작하면 2초 대기 시간

        NoteSpawn();

        NoteMove();
    }

    // 노트의 시간과 지금 시간을 비교하며 조건에 맞을 경우 노트를 생성하는 함수
    private void NoteSpawn()
    {
        if (notes.Count == count) return; // 예외처리

        if (notes[count].time <= GameManager.instance.currentTime)
        {
            if (notes[count].haveGameObj) return;

            Note note = GetNote(notes[count].noteEnum);

            note.angle = notes[count].angle;
            note.time = notes[count].time;
            note.haveGameObj = true;

            note.transform.rotation = Quaternion.Euler(new Vector3(0, 0, note.angle - 90));

            float cos = Mathf.Cos((90 - note.angle) * Mathf.Deg2Rad);
            float sin = Mathf.Sin((90 - note.angle) * Mathf.Deg2Rad);

            note.transform.position = new Vector2(sin, cos) * noteLength;

            notes[count] = note;

            count++;

            NoteSpawn(); // 같은 시간에 노트가 나올 수 있으니 함수를 한번더 호출
        }
    }

    // 활성화 되어있는 노트를 원으로 이동 시키는 함수
    private void NoteMove()
    {
        for (int i = 0; i < notes.Count; i++)
        {
            if(notes[i].haveGameObj)
            {
                Vector3 dir = (GameManager.instance.circle.transform.position - notes[i].transform.position).normalized;
                notes[i].transform.position += dir * Time.deltaTime * noteSpeed;
            }
        }
    }

    // SetNote 함수를 통해 노트의 패턴을 생성하는 함수 ---------------------------------------------------------------------
    private void SetNotePattern()
    {
        for(int i = 0; i < 1000; i++)
        {
            SetNote(NoteEnum.Single, 0, 2 + (i * 0.5f));
        }

    }
    // 노트를 언제 어디서 생성할지의 정보를 notes에 넣어주는 함수
    public static void SetNote(NoteEnum noteEnum, float angle, float time)
    {
        Note note = new Note();

        note.noteEnum = noteEnum;
        note.angle = angle;
        note.time = time;
        note.haveGameObj = false;

        instance.notes.Add(note);
    }

    public static void RemoveNote(Note removeNote)
    {
        removeNote.gameObject.SetActive(false);
        removeNote.transform.position = new Vector3(0, 1000, 0); // 오류 방지 코드

        removeNote.haveGameObj = false;

        Debug.Log("bool : " + instance.notes.Remove(removeNote)); // 절대 지우면 안되는 코드

        instance.count--;
    }

    private Note GetNote(NoteEnum noteEnum)
    {
        Note note = null;

        switch (noteEnum)
        {
            case NoteEnum.DC:
                note = PoolManager.GetItem<DCNote>(instance.dcNotePrefab);
                break;
            case NoteEnum.Single:
                note = PoolManager.GetItem<SingleNote>(instance.singleNotePrefab);
                break;
            case NoteEnum.Long:
                note = PoolManager.GetItem<LongNote>(instance.longNotePrefab);
                break;
        }

        if (note == null)
        {
            Debug.LogError("정해진 종류 이외의 노트를 선택하였습니다");
        }

        note.gameObject.SetActive(true);

        return note;
    }
}
