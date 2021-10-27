using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SaveAndLoadManager : MonoBehaviour
{
    public static SaveAndLoadManager instance;

    public Button saveButton;
    public Button loadButton;

    private string filePath;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("여러개의 SaveAndLoadManager가 생성되었습니다");
        }
        instance = this;
    }

    private void Start()
    {
        filePath = Application.persistentDataPath + "/" + "test.txt";

        if(saveButton != null)
        {
            saveButton.onClick.AddListener(() =>
            {
                Save();
            });

            loadButton.onClick.AddListener(() =>
            {
                Load();
            });
        }

    }

    private void Save()
    {
        File.WriteAllText(filePath, NoteManager.instance.NotesToJson());

        Debug.Log(filePath);
    }

    private void Load()
    {
        if(File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            NoteManager.instance.NotesToJson();

            Debug.Log("Load Json : " + json);

            NoteVOList noteList = JsonUtility.FromJson<NoteVOList>("{\"notes\":" + json + "}");

            NoteManager.instance.NotesClear();

            Note note = null;

            for(int i = 0; i < noteList.notes.Length; i++)
            {
                switch(noteList.notes[i].noteEnum)
                {
                    case NoteManager.NoteEnum.DC:
                        note = PoolManager.GetItem<DCNote>(NoteManager.instance.dcNotePrefab);
                        break;
                    case NoteManager.NoteEnum.Single:
                        note = PoolManager.GetItem<SingleNote>(NoteManager.instance.singleNotePrefab);
                        break;
                    case NoteManager.NoteEnum.Long:
                        note = PoolManager.GetItem<LongNote>(NoteManager.instance.longNotePrefab);
                        break;
                }

                note.angle = noteList.notes[i].angle;
                note.time = noteList.notes[i].time;

                NoteManager.instance.AddNote(note);
                NoteManager.instance.SetNoteValue(note);
            }

            NoteManager.instance.NotesSort();
        }
    }

    //for(int i = 0; i < noteList.notes.Length; i++)
    //{
    //    Debug.Log(noteList.notes[i] + " " + noteList.notes[i].angle + " " + noteList.notes[i].time + " " + noteList.notes[i].noteEnum);
    //}
}
