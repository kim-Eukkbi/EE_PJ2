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
        if (GameManager.instance.waitTime > 0f) return; // �����ϸ� 2�� ��� �ð�

        NoteMove();
    }

    // Ȱ��ȭ �Ǿ��ִ� ��Ʈ�� ������ �̵� ��Ű�� �Լ�
    private void NoteMove()
    {
        for (int i = 0; i < notes.Count; i++)
        {
            notes[i].transform.position += (notes[i].transform.up * -1) * Time.deltaTime * noteSpeed;
        }
    }

    // SetNote �Լ��� ���� ��Ʈ�� ������ �����ϴ� �Լ� ---------------------------------------------------------------------
    private void SetNotePattern()
    {
        for(int i = 0; i < 10; i++)
        {
            SetNote(NoteEnum.DC, i * 100, i * 0.1f);
        }
    }
    // ��Ʈ�� ���� ��� ���������� ������ notes�� �־��ִ� �Լ�
    public static void SetNote(NoteEnum noteEnum, float angle, float time)
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

    public static void RemoveNote(Note removeNote)
    {
        removeNote.gameObject.SetActive(false);
        removeNote.transform.position = new Vector3(0, 1000, 0); // ���� ���� �ڵ�

        instance.notes.Remove(removeNote);
        //Debug.Log("bool : " + instance.notes.Remove(removeNote)); // ���� ����� �ȵǴ� �ڵ�
        //Debug.Log("note Time : " + removeNote.time);
        //Debug.Log("gameManager Time : " + GameManager.instance.currentTime);

        instance.count--;
    }
}
