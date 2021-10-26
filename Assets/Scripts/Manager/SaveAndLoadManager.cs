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
            Debug.Log("Load Json : " + json);

            NoteVOList noteList = JsonUtility.FromJson<NoteVOList>(json);
            
            for(int i = 0; i < noteList.notes.Count; i++)
            {
                Debug.Log(noteList.notes[i]);
            }
        }
    }
}
