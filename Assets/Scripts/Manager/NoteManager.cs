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

        //for (int i = 0; i < notes.Count; i++)
        //{
        //    Debug.Log(notes[i].time);
        //}
    }

    private void Update()
    {
        if (GameManager.instance.currentTime < 2f) return; // �����ϸ� 2�� ��� �ð�

        NoteSpawn();

        NoteMove();
    }

    // ��Ʈ�� �ð��� ���� �ð��� ���ϸ� ���ǿ� ���� ��� ��Ʈ�� �����ϴ� �Լ�
    private void NoteSpawn()
    {
        if (notes.Count == count) return; // ����ó��

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

            NoteSpawn(); // ���� �ð��� ��Ʈ�� ���� �� ������ �Լ��� �ѹ��� ȣ��
        }
    }

    // Ȱ��ȭ �Ǿ��ִ� ��Ʈ�� ������ �̵� ��Ű�� �Լ�
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

    // SetNote �Լ��� ���� ��Ʈ�� ������ �����ϴ� �Լ� ---------------------------------------------------------------------
    private void SetNotePattern()
    {
        for(int i = 0; i < 1000; i++)
        {
            SetNote(NoteEnum.Single, 0, 2 + (i * 0.5f));
        }

    }
    // ��Ʈ�� ���� ��� ���������� ������ notes�� �־��ִ� �Լ�
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
        removeNote.transform.position = new Vector3(0, 1000, 0); // ���� ���� �ڵ�

        removeNote.haveGameObj = false;

        Debug.Log("bool : " + instance.notes.Remove(removeNote)); // ���� ����� �ȵǴ� �ڵ�

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
            Debug.LogError("������ ���� �̿��� ��Ʈ�� �����Ͽ����ϴ�");
        }

        note.gameObject.SetActive(true);

        return note;
    }
}
