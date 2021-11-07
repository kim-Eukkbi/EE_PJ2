using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class NoteManager : MonoBehaviour
{
    public static NoteManager instance;

    public GameObject dcNotePrefab;
    public GameObject singleNotePrefab;
    public GameObject longNotePrefab;
    public Transform parent;

    public float noteSpeed = 1;
    public float noteRenderLength = 10f;

    private List<Note> notes;
    private List<Note> havingNotes;

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
        havingNotes = new List<Note>();
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

    // 디버그용 함수
    public void NoteDebug()
    {
        Debug.Log(notes.Count);
    }

    // 활성화 되어있는 노트를 원으로 이동 시키는 함수
    private void NoteMove()
    {
        for (int i = 0; i < notes.Count; i++)
        {
            // 원에 노트가 닿지않았고, 노트의 최대 렌더거리보다 안쪽에 있다면 그 노트는 활성화
            if (!(notes[i].time < AudioManager.instance.musicCurrentTime || !(notes[i].time < AudioManager.instance.musicCurrentTime + noteRenderLength)))
            {
                notes[i].gameObject.SetActive(true);
            }
            else if(!notes[i].gameObject.activeSelf) // 활성화 되어 있지 않은 경우는 계산하지 않음
            {
                continue;
            }

            // 노트의 시간 - 지금 시간을 빼서 그에 따른 위치에 이동시킨다
            notes[i].transform.position = notes[i].transform.up * instance.noteSpeed * (notes[i].time - AudioManager.instance.musicCurrentTime);
            notes[i].transform.position += notes[i].transform.up *
                                    (GameManager.instance.judgeLineY + GameManager.instance.judgeLine.transform.localScale.y);
        }
    }

    // SetSpawnNote 함수를 통해 노트의 패턴을 생성하는 함수 ---------------------------------------------------------------------
    private void SetNotePattern()
    {
        // 스테이지의 이름을 가진 파일을 가져와서 생성 하면 좋지 않을까?

        if(!GameManager.instance.isEditerMode)
        {
            string fileName = GameManager.instance.stageName;
            string filePath = SaveAndLoadManager.instance.GetFilePath();
            if (File.Exists(filePath + GameManager.instance.stageName))
            {
                string json = File.ReadAllText(filePath + fileName);

                NoteVOList noteList = JsonUtility.FromJson<NoteVOList>("{\"notes\":" + json + "}");

                NotesClear();

                // 받은 json의 noteEnum에 따라서 다른 노트들을 생성한다
                for (int i = 0; i < noteList.notes.Length; i++)
                {
                    SetSpawnNote(noteList.notes[i].noteEnum, noteList.notes[i].angle, noteList.notes[i].time);
                }

                NotesSort();
            }
        }
        else
        {
            for (int i = 0; i < havingNotes.Count; i++)
            {
                notes.Add(havingNotes[i]);
            }

            havingNotes.Clear();
        }
    }

    // 지금 진행되는 시간에 맞춰 노트들을 이동시키는 함수
    public void SetNoteTimePosition()
    {
        for(int i = 0; i < notes.Count; i++)
        {
            // 원에 노트가 닿거나, 노트의 최대 렌더거리보다 멀리있다면 그 노트는 비활성화
            if (notes[i].time < AudioManager.instance.musicCurrentTime || !(notes[i].time < AudioManager.instance.musicCurrentTime + noteRenderLength))
            {
                notes[i].gameObject.SetActive(false);
                continue;
            }
            else
            {
                notes[i].gameObject.SetActive(true);
            }

            notes[i].transform.position = notes[i].transform.up * instance.noteSpeed * (notes[i].time - AudioManager.instance.musicCurrentTime);
            notes[i].transform.position += notes[i].transform.up *
                                    (GameManager.instance.judgeLineY + GameManager.instance.judgeLine.transform.localScale.y);
        }
    }

    // 노트를 언제 어디서 생성할지의 정보를 notes에 넣어주는 함수
    public void SetSpawnNote(NoteEnum noteEnum, float angle, float time)
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
        note.angle = angle % 360;
        note.time = time;

        note.transform.rotation = Quaternion.Euler(0, 0, angle % 360);
        note.transform.position = note.transform.up * instance.noteSpeed * (time);
        note.transform.position += note.transform.up * 
                                (GameManager.instance.judgeLineY + GameManager.instance.judgeLine.transform.localScale.y);

        notes.Add(note);

        SetNoteValue(note);
    }

    public void SetHavingNotes()
    {
        for(int i = 0; i < notes.Count; i++)
        {
            havingNotes.Add(notes[i]);
        }
    }

    // 받은 인수의 angle과 time에 알맞게 위치를 변경해주는 함수
    public void SetNoteValue(Note note)
    {
        if (note == null) return;

        Note findNote = notes.Find(x => x.time == note.time);

        if(findNote != null)
        {
            note.transform.rotation = Quaternion.Euler(0, 0, note.angle);

            note.transform.position = note.transform.up * instance.noteSpeed * (note.time - AudioManager.instance.musicCurrentTime);
            note.transform.position += note.transform.up *
                                    (GameManager.instance.judgeLineY + GameManager.instance.judgeLine.transform.localScale.y);
        }
    }

    // 받은 인수의 노트를 notes에다 추가해주는 함수
    public void AddNote(Note addNote)
    {
        notes.Add(addNote);
    }

    // 받은 인수에 해당하는 노트를 지우는 함수
    public void RemoveNote(Note removeNote)
    {
        removeNote.gameObject.SetActive(false);
        removeNote.transform.position = new Vector3(0, 1000, 0); // 오류 방지 코드

        notes.Remove(removeNote);
    }

    // 게임안에 있는 노트들을 다시 처음으로 리셋시키는 함수
    public void NotesReset()
    {
        NotesClear();

        SetNotePattern();
    }

    // 모든 노트들을 비활성화 시키며 다 클리어 시키는 함수
    public void NotesClear()
    {
        for (int i = 0; i < notes.Count; i++)
        {
            notes[i].gameObject.SetActive(false);
            notes[i].transform.position = new Vector3(0, 1000, 0);
            RemoveNote(notes[i]);
            i--;
        }
    }

    public void NotesSort()
    {
        notes.Sort((x, y) => x.time.CompareTo(y.time));
    }

    // notes에 있는 노트들의 정보를 json으로 변환해서 리턴해주는 함수
    public string NotesToJson()
    {
        string json = "";

        notes.Sort((x, y) => x.time.CompareTo(y.time));

        json += "[";

        for(int i = 0; i < notes.Count; i++)
        {
            json += JsonUtility.ToJson(notes[i]) + (i == notes.Count - 1 ? "" : ", ");
        }

        json += "]";

        return json;
    }
}
