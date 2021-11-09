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
            Debug.LogError("NoteManager�� ������ �����Ǿ����ϴ�");
        }
        instance = this;

        notes = new List<Note>();
        havingNotes = new List<Note>();
    }

    private void Start()
    {
        // ������ ��Ʈ���� Ǯ �����
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

    // ����׿� �Լ�
    public void NoteDebug()
    {
        Debug.Log(notes.Count);
    }

    // Ȱ��ȭ �Ǿ��ִ� ��Ʈ�� ������ �̵� ��Ű�� �Լ�
    private void NoteMove()
    {
        for (int i = 0; i < notes.Count; i++)
        {
            // ���� ��Ʈ�� �����ʾҰ�, ��Ʈ�� �ִ� �����Ÿ����� ���ʿ� �ִٸ� �� ��Ʈ�� Ȱ��ȭ
            if (!(notes[i].time < AudioManager.instance.musicCurrentTime || !(notes[i].time < AudioManager.instance.musicCurrentTime + noteRenderLength)))
            {
                notes[i].gameObject.SetActive(true);
            }
            else if(!notes[i].gameObject.activeSelf) // Ȱ��ȭ �Ǿ� ���� ���� ���� ������� ����
            {
                continue;
            }

            // ��Ʈ�� �ð� - ���� �ð��� ���� �׿� ���� ��ġ�� �̵���Ų��
            notes[i].transform.position = notes[i].transform.up * instance.noteSpeed * (notes[i].time - AudioManager.instance.musicCurrentTime);
            notes[i].transform.position += notes[i].transform.up *
                                    (GameManager.instance.judgeLineY + GameManager.instance.judgeLine.transform.localScale.y);
        }
    }

    // SetSpawnNote �Լ��� ���� ��Ʈ�� ������ �����ϴ� �Լ� ---------------------------------------------------------------------
    private void SetNotePattern()
    {
        // ���������� �̸��� ���� ������ �����ͼ� ���� �ϸ� ���� ������?

        if(!GameManager.instance.isEditerMode)
        {
            string fileName = GameManager.instance.stageName;
            string filePath = SaveAndLoadManager.instance.GetFilePath();
            if (File.Exists(filePath + GameManager.instance.stageName))
            {
                string json = File.ReadAllText(filePath + fileName);

                NoteVOList noteList = JsonUtility.FromJson<NoteVOList>("{\"notes\":" + json + "}");

                NotesClear();

                // ���� json�� noteEnum�� ���� �ٸ� ��Ʈ���� �����Ѵ�
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

    // ���� ����Ǵ� �ð��� ���� ��Ʈ���� �̵���Ű�� �Լ�
    public void SetNoteTimePosition()
    {
        for(int i = 0; i < notes.Count; i++)
        {
            // ���� ��Ʈ�� ��ų�, ��Ʈ�� �ִ� �����Ÿ����� �ָ��ִٸ� �� ��Ʈ�� ��Ȱ��ȭ
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

    // ��Ʈ�� ���� ��� ���������� ������ notes�� �־��ִ� �Լ�
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

    // ���� �μ��� angle�� time�� �˸°� ��ġ�� �������ִ� �Լ�
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

    // ���� �μ��� ��Ʈ�� notes���� �߰����ִ� �Լ�
    public void AddNote(Note addNote)
    {
        notes.Add(addNote);
    }

    // ���� �μ��� �ش��ϴ� ��Ʈ�� ����� �Լ�
    public void RemoveNote(Note removeNote)
    {
        removeNote.gameObject.SetActive(false);
        removeNote.transform.position = new Vector3(0, 1000, 0); // ���� ���� �ڵ�

        notes.Remove(removeNote);
    }

    // ���Ӿȿ� �ִ� ��Ʈ���� �ٽ� ó������ ���½�Ű�� �Լ�
    public void NotesReset()
    {
        NotesClear();

        SetNotePattern();
    }

    // ��� ��Ʈ���� ��Ȱ��ȭ ��Ű�� �� Ŭ���� ��Ű�� �Լ�
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

    // notes�� �ִ� ��Ʈ���� ������ json���� ��ȯ�ؼ� �������ִ� �Լ�
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
