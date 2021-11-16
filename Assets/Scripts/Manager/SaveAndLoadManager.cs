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

    public string nullStageFilePath;
    
    private string filePath;

    private string fileName;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("�������� SaveAndLoadManager�� �����Ǿ����ϴ�");
        }
        instance = this;
    }

    private void Start()
    {
        filePath = Application.persistentDataPath + "/noteFiles/";

        nullStageFilePath = Application.persistentDataPath + "/stages/";

        if (saveButton != null)
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

    public string GetFilePath()
    {
        return filePath;
    }

    public string GetStageFilePath()
    {
        return filePath + "/stages/";
    }

    public void SetFilePath(string newFilepath)
    {
        filePath = newFilepath;
    }

    private void Save()
    {
        if(!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        fileName = FileUIManager.instance.GetFileName();

        if (fileName == "") return;

        File.WriteAllText(filePath + fileName, NoteManager.instance.NotesToJson());

        FileUIManager.instance.FileUIActive(false);
    }

    private void Load()
    {
        fileName = FileUIManager.instance.GetFileName(); // ���� ������ �̸���������

        if (fileName == null) return; // ���� ���� ���� ��� ����

        if (File.Exists(filePath + fileName))
        {
            string json = File.ReadAllText(filePath + fileName);

            Debug.Log("Load Json : " + json);

            Debug.Log(filePath);

            NoteVOList noteList = JsonUtility.FromJson<NoteVOList>("{\"notes\":" + json + "}");

            NoteManager.instance.NotesClear();

            // ���� json�� noteEnum�� ���� �ٸ� ��Ʈ���� �����Ѵ�
            for (int i = 0; i < noteList.notes.Length; i++)
            {
                OptionManager.instance.GetCreateNote(noteList.notes[i].angle, noteList.notes[i].time, noteList.notes[i].noteEnum);
            }

            NoteManager.instance.NotesSort();

            FileUIManager.instance.FileUIActive(false);
        }
    }

    //for(int i = 0; i < noteList.notes.Length; i++)
    //{
    //    Debug.Log(noteList.notes[i] + " " + noteList.notes[i].angle + " " + noteList.notes[i].time + " " + noteList.notes[i].noteEnum);
    //}
}
