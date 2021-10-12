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
        PoolManager.CreatePool<Note>(longNotePrefab, parent, 30);
    }

    float current = 0;
    private void Update()
    {
        if(GameManager.instance.currentTime > current)
        {
            CreateLongNote(NoteEnum.DC, currentAngle, 30);
            current += spawnTime;
        }

        for(int i = 0; i < notes.Count; i++)
        {
            Vector3 dir = (GameManager.instance.Circle.transform.position - notes[i].transform.position).normalized;
            notes[i].transform.position += dir * Time.deltaTime;
        }
    }

    // 노트 생성
    public static void CreateLongNote(NoteEnum noteEnum, float angle, float time, float length = 0)
    {
        Note note = PoolManager.GetItem<Note>(instance.longNotePrefab);
        instance.notes.Add(note);

        note.noteEnum = noteEnum;
        note.angle = angle;
        note.time = time;
        note.length = length;

        note.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

        float cos = Mathf.Cos((90 - angle) * Mathf.Deg2Rad);
        float sin = Mathf.Sin((90 - angle) * Mathf.Deg2Rad);

        note.transform.position = new Vector2(sin, cos) * 5;
    }

}
