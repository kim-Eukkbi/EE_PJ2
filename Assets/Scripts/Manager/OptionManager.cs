using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public static OptionManager instance;

    public Button startBtn;
    public Button stopBtn;

    [Header("Start And Stop")]
    public GameObject startAndStopPanel;

    [Header("Game Information")]
    public GameObject gameInformationPanel;
    public InputField noteSpeedIF;
    public InputField countDownIF;

    [Header("Note Information")]
    public GameObject noteInformationPanel;
    public InputField noteAngleIF;
    public InputField noteTimeIF;
    public Dropdown noteEnumDropdown;
    public Button removeNoteButton;

    [Header("Note Add")]
    public GameObject noteAddPanel;
    public Button noteAddButton;

    [Header("SaveAndLoad")]
    public GameObject saveAndLoadPanel;

    [Header("Panel Move Buttons")]
    public GameObject PanelMoveButtonsPanel;
    public Button startAndStopMoveBtn;
    public Button gameInformationMoveBtn;
    public Button noteInformationMoveBtn;
    public Button noteAddMoveBtn;
    public Button saveAndLoadMoveBtn;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("OptionManager가 여러개 생성되었습니다");
        }
        instance = this;
    }
    void Start()
    {
        if (stopBtn == null) // 잠시 그 뭐냐 그냥 잠시 쓰는거
        {
            GameManager.instance.GameStart();
        }
        if (startBtn != null)
        {
            startBtn.onClick.AddListener(() =>
            {
                GameManager.instance.GameStart();
            });
        }

        if (stopBtn != null)
        {
            stopBtn.onClick.AddListener(() =>
            {
                GameManager.instance.GameReset();
            });
        }

        if (noteAddPanel != null)
        {
            noteAddButton.onClick.AddListener(() =>
            {
                Note note = PoolManager.GetCreateItem<DCNote>(NoteManager.instance.dcNotePrefab);

                note.angle = 0;
                note.time = GameManager.instance.countDownTime;
                note.noteEnum = NoteManager.NoteEnum.DC;

                noteAngleIF.text = note.angle.ToString();
                noteTimeIF.text = note.time.ToString();

                note.gameObject.SetActive(true);

                NoteManager.instance.AddNote(note);

                SelectNoteManager.instance.SelectNote = note;

                Debug.Log("ADDDDDDD NOOOOOOOTOTOTETETEEE");

                UISetActive(2);
            });
        }

        if (PanelMoveButtonsPanel != null)
        {
            if (startAndStopMoveBtn != null)
            {
                startAndStopMoveBtn.onClick.AddListener(() =>
                {
                    UISetActive(0);
                });
            }

            if (gameInformationMoveBtn != null)
            {
                gameInformationMoveBtn.onClick.AddListener(() =>
                {
                    UISetActive(1);
                });

                removeNoteButton.onClick.AddListener(() =>
                {
                    if (SelectNoteManager.instance.SelectNote != null)
                    {
                        NoteManager.instance.RemoveNote(SelectNoteManager.instance.SelectNote);

                        SelectNoteManager.instance.SelectNote = null;
                    }
                });
            }

            if (noteInformationMoveBtn != null)
            {
                noteInformationMoveBtn.onClick.AddListener(() =>
                {
                    UISetActive(2);
                });
            }

            if (noteAddMoveBtn != null)
            {
                noteAddMoveBtn.onClick.AddListener(() =>
                {
                    UISetActive(3);
                });
            }

            if (saveAndLoadMoveBtn != null)
            {
                saveAndLoadMoveBtn.onClick.AddListener(() =>
                {
                    UISetActive(4);
                });
            }
        }
    }

    private void Update()
    {
        if (gameInformationPanel != null)
        {
            if (gameInformationPanel.activeSelf)
            {
                int result = 0;

                if (int.TryParse(noteSpeedIF.text, out result))
                {
                    NoteManager.instance.noteSpeed = result;
                }

                if (int.TryParse(countDownIF.text, out result))
                {
                    GameManager.instance.countDownTime = result;
                }
            }
        }

        if (noteInformationPanel != null)
        {
            if (noteInformationPanel.activeSelf)
            {
                if (float.TryParse(noteAngleIF.text, out float angle) && float.TryParse(noteTimeIF.text, out float time))
                {
                    if (time < 0.3f)
                    {
                        noteTimeIF.text = "0.3";
                        time = 0.3f;
                    }
                    if (SelectNoteManager.instance.SelectNote != null)
                    {
                        Note note = SelectNoteManager.instance.SelectNote;

                        note.angle = angle;
                        note.time = time;
                        note.noteEnum = (NoteManager.NoteEnum)(noteEnumDropdown.value + 7);

                        SelectNoteManager.instance.SelectNote = note;
                    }
                }
                else
                {
                    noteAngleIF.text = noteAngleIF.text == "" ? "0" : noteAngleIF.text;
                    //noteTimeIF.text = noteTimeIF.text == "" ? "0" : noteTimeIF.text;
                }

                if (removeNoteButton != null)
                {
                    if (SelectNoteManager.instance.SelectNote != null)
                    {
                        removeNoteButton.gameObject.SetActive(true);
                    }
                    else
                    {
                        removeNoteButton.gameObject.SetActive(false);
                    }
                }
            }

            noteEnumDropdown.onValueChanged.AddListener((int index) =>
            {
                if ((int)SelectNoteManager.instance.SelectNote.noteEnum == 7 + index)
                {
                    return;
                }

                float angle = SelectNoteManager.instance.SelectNote.angle;
                float time = SelectNoteManager.instance.SelectNote.time;

                Debug.Log(SelectNoteManager.instance.SelectNote);



                NoteManager.instance.RemoveNote(SelectNoteManager.instance.SelectNote);

                switch (index)
                {
                    case 0:
                        SelectNoteManager.instance.SelectNote = GetCreateNote(angle, time, NoteManager.NoteEnum.DC);
                        break;
                    case 1:
                        SelectNoteManager.instance.SelectNote = GetCreateNote(angle, time, NoteManager.NoteEnum.Single);
                        break;
                    case 2:
                        SelectNoteManager.instance.SelectNote = GetCreateNote(angle, time, NoteManager.NoteEnum.Long);
                        break;
                }
            });
        }
    }

    public void SetPanels(Note note)
    {
        if (note == null)
        {
            noteAngleIF.text = "";
            noteTimeIF.text = "";
            noteEnumDropdown.value = 0;

            return;
        }

        noteAngleIF.text = note.angle.ToString();
        noteTimeIF.text = note.time.ToString();
        noteEnumDropdown.value = ((int)note.noteEnum) - 7;

        UISetActive(2);
    }

    /// <summary>
    /// num == 0 : startAndStopPanel 활성화,
    /// num == 1 : gameInformationPanel 활성화,
    /// num == 2 : noteInformationPanel 활성화
    /// </summary>
    /// <param name="num"></param>
    private void UISetActive(int num)
    {
        startAndStopPanel.SetActive(num == 0);
        gameInformationPanel.SetActive(num == 1);
        noteInformationPanel.SetActive(num == 2);
        noteAddPanel.SetActive(num == 3);
        saveAndLoadPanel.SetActive(num == 4);
    }

    private Note GetCreateNote(float angle, float time, NoteManager.NoteEnum noteEnum)
    {
        Note note = null;

        Debug.Log("매우 많이 생성중");

        switch (noteEnum)
        {
            case NoteManager.NoteEnum.DC:
                note = PoolManager.GetCreateItem<DCNote>(NoteManager.instance.dcNotePrefab);
                break;
            case NoteManager.NoteEnum.Single:
                note = PoolManager.GetCreateItem<SingleNote>(NoteManager.instance.singleNotePrefab);
                break;
            case NoteManager.NoteEnum.Long:
                note = PoolManager.GetCreateItem<LongNote>(NoteManager.instance.longNotePrefab);
                break;
        }

        note.time = time;
        note.angle = angle;
        note.noteEnum = noteEnum;

        NoteManager.instance.AddNote(note);

        NoteManager.instance.SetNoteValue(note);

        return note;
    }
}
