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

    public float spawnTime = 0.2f;
    public float currentAngle = 90;
    public float noteSpeed = 1;
    public float rotationSpeed = 1;
    public float length = 5;

    
    private List<Note> notes;

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
        PoolManager.CreatePool<Note>(dcNotePrefab, parent, 30);
    }

    float current = 0;
    private void Update()
    {
        if (GameManager.instance.currentTime < 2f) return;

        if (GameManager.instance.currentTime > current + 2)
        {
            CreateNote(NoteEnum.DC, currentAngle, 30);
            current += spawnTime;
            currentAngle += 1.5f * rotationSpeed;
        }

        NoteMove();
    }

    private void NoteMove()
    {
        for (int i = 0; i < notes.Count; i++)
        {
            Vector3 dir = (GameManager.instance.circle.transform.position - notes[i].transform.position).normalized;
            notes[i].transform.position += dir * Time.deltaTime * noteSpeed;    
        }
    }

    // 노트 생성
    public static void CreateNote(NoteEnum noteEnum, float angle, float time, float length = 0)
    {
        Note note = PoolManager.GetItem<Note>(instance.dcNotePrefab);
        instance.notes.Add(note);

        note.noteEnum = noteEnum;
        note.angle = angle;
        note.time = time;
        note.length = length;

        note.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

        float cos = Mathf.Cos((90 - angle) * Mathf.Deg2Rad);
        float sin = Mathf.Sin((90 - angle) * Mathf.Deg2Rad);

        note.transform.position = new Vector2(sin, cos) * instance.length;
    }

    public static void RemoveNote(GameObject removeNote)
    {
        removeNote.SetActive(false);
        removeNote.transform.position = new Vector3(0, 1000, 0); // 오류 방지용으로 오브젝트를 올려줌

        Note note = instance.notes.Find(i => !i.gameObject.activeSelf);
        instance.notes.Remove(note);
    }
}
