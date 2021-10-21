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

    /// <summary>
    /// DC == DontClick,
    /// Single == One Click, 
    /// Long == Long Note
    /// </summary>
    public enum NoteEnum
    {
        DC = 7,
        Single = 8,
        Long = 9
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
    }

    private void Update()
    {
        if (!GameManager.instance.isGameStart)
        {
            SetNoteTimePosition();
            return;
        }

        NoteMove();
    }

    // 활성화 되어있는 노트를 원으로 이동 시키는 함수
    private void NoteMove()
    {
        for (int i = 0; i < notes.Count; i++)
        {
            notes[i].transform.position += (notes[i].transform.up * -1) * Time.deltaTime * noteSpeed;
        }
    }

    // SetNote 함수를 통해 노트의 패턴을 생성하는 함수 ---------------------------------------------------------------------
    private void SetNotePattern()
    {
        for (int i = 0; i < 100; i++)
        {
            SetNote(NoteEnum.Single, i * 30, 3 + i * 0.3f);
        }
    }

    public void SetNoteTimePosition()
    {
        for(int i = 0; i < notes.Count; i++)
        {
            if (notes[i].time < AudioManager.instance.musicCurrentTime)
            {
                notes[i].gameObject.SetActive(false);
                continue;
            }
            else
            {
                notes[i].gameObject.SetActive(true);
            }

            notes[i].transform.position = notes[i].transform.up * instance.noteSpeed * (notes[i].time);
            notes[i].transform.position += notes[i].transform.up *
                                    (0.8f + GameManager.instance.judgeLine.transform.localScale.y);
            notes[i].transform.position -= notes[i].transform.up * instance.noteSpeed * AudioManager.instance.musicCurrentTime;
        
        
        }
    }

    // 노트를 언제 어디서 생성할지의 정보를 notes에 넣어주는 함수
    public void SetNote(NoteEnum noteEnum, float angle, float time)
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

        note.noteEnum = noteEnum;
        note.angle = angle;
        note.time = time;

        note.transform.rotation = Quaternion.Euler(0, 0, angle);
        note.transform.position = note.transform.up * instance.noteSpeed * (time);
        note.transform.position += note.transform.up * 
                                (GameManager.instance.judgeLine.transform.position.y + GameManager.instance.judgeLine.transform.localScale.y);

        //string json = JsonUtility.ToJson(note);

        //Debug.Log("json : " + json);

        //Debug.Log(JsonUtility.FromJson<NoteVO>(json).noteEnum);

        instance.notes.Add(note);
    }

    public void RemoveNote(Note removeNote)
    {
        removeNote.gameObject.SetActive(false);
        removeNote.transform.position = new Vector3(0, 1000, 0); // 오류 방지 코드

        instance.notes.Remove(removeNote);
    }

    public void NotesReset()
    {
        for(int i = 0; i < instance.notes.Count; i++)
        {
            instance.notes[i].gameObject.SetActive(false);
            instance.notes[i].transform.position = new Vector3(0, 1000, 0);
            instance.RemoveNote(instance.notes[i]);
            i--;
        }

        instance.SetNotePattern();
    }
}
