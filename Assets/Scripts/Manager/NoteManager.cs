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
    public float noteRenderLength = 10f;

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
            Debug.LogError("NoteManager�� ������ �����Ǿ����ϴ�");
        }
        instance = this;

        notes = new List<Note>();
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

            Debug.Log(notes[i].time);

            // ��Ʈ�� �ð� - ���� �ð��� ���� �׿� ���� ��ġ�� �̵���Ų��
            notes[i].transform.position = notes[i].transform.up * instance.noteSpeed * (notes[i].time - AudioManager.instance.musicCurrentTime);
            notes[i].transform.position += notes[i].transform.up *
                                    (GameManager.instance.judgeLineY + GameManager.instance.judgeLine.transform.localScale.y);
        }
    }

    // SetSpawnNote �Լ��� ���� ��Ʈ�� ������ �����ϴ� �Լ� ---------------------------------------------------------------------
    private void SetNotePattern()
    {
        for (int i = 0; i < 1000; i++)
        {
            if(i % 3 == 0)
            {
                SetSpawnNote(NoteEnum.DC, 5 + (i * 2), 3 + i * 0.5f);
            }
            else if(i % 3 == 1)
            {
                SetSpawnNote(NoteEnum.Single, 5 + (i * 2), 3 + i * 0.5f);
            }
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

        instance.notes.Add(note);
    }

    // notes����Ʈ�� ���� �ڿ��ִ� ���� �����ϴ� �Լ�
    public Note GetBackNote()
    {
        return notes[notes.Count - 1];
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

        instance.notes.Remove(removeNote);
    }

    // ���Ӿȿ� �ִ� ��Ʈ���� �ٽ� ó������ ���½�Ű�� �Լ�
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

    // notes�� �ִ� ��Ʈ���� ������ json���� ��ȯ�ؼ� �������ִ� �Լ�
    public string NotesToJson()
    {
        string json = "";

        notes.Sort((x, y) => x.time.CompareTo(y.time));

        json += "\"notes\":[";

        for(int i = 0; i < notes.Count; i++)
        {
            json += JsonUtility.ToJson(notes[i]) + (i == notes.Count - 1 ? "" : ", ");
        }

        json += "]";

        return json;
    }
}
