using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConvenienceManager : MonoBehaviour
{
    public static ConvenienceManager instance;

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
            if (InputManager.instance.NoteSpawnKeyDown)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                float dis = Vector3.Distance(mousePos, GameManager.instance.circle.transform.position)
                                            - GameManager.instance.judgeLineY
                                            - GameManager.instance.judgeLine.transform.localScale.y;

                OptionManager.instance.GetCreateNote(circleAngle, AudioManager.instance.musicLength * AudioManager.instance.scrollbar.value + (dis / NoteManager.instance.noteSpeed), NoteManager.NoteEnum.Single);
                OptionManager.instance.UISetActive(2);
            }
        }
        else
        {
            if(SpawnNotesToggle.isOn)
            {
                if (InputManager.instance.NoteSpawnKeyDown)
                {
                    SingleNote note = PoolManager.GetCreateItem<SingleNote>(NoteManager.instance.singleNotePrefab);

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
}
