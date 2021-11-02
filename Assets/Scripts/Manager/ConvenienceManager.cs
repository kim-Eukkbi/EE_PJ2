using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConvenienceManager : MonoBehaviour
{
    public static ConvenienceManager instance;

    public LayerMask whatIsDCNote;
    public LayerMask whatIsSingleNote;
    public LayerMask whatIsLongNote;

    public GameObject testObj;
    public Toggle SpawnNotesToggle;

    private float circleAngle;
    private List<Note> startingNotes = new List<Note>();

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
        if (circleAngle < 0)
        {
            circleAngle += 360;
        }

        if (!GameManager.instance.isGameStart)
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
        else
        {
            if(SpawnNotesToggle.isOn)
            {
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

    public void SetStartingNotes()
    {
        for(int i = 0; i < startingNotes.Count; i++)
        {
            NoteManager.instance.AddNote(startingNotes[i]);
            NoteManager.instance.SetNoteValue(startingNotes[i]);
        }

        startingNotes.Clear();
    }

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
