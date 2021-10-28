using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class FileUIManager : MonoBehaviour
{
    public static FileUIManager instance;

    public GameObject fileUIPanel;
    public GameObject content;
    public GameObject filePathPrefab;
    public InputField filePathInputField;

    public Button fileUIOpenButton;

    private List<Button> fileButtonList = new List<Button>();
    private List<string> fileList = new List<string>();

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("FileUIManager가 여러개 선언되었습니다");
        }
        instance = this;
    }

    private void Start()
    {
        fileUIOpenButton.onClick.AddListener(() =>
        {
            OpenFileUI(SaveAndLoadManager.instance.GetFilePath());
        });
    }

    private void OpenFileUI(string path)
    {
        string[] files = Directory.GetFiles(path);

        if (files.Length != 0)
        {
            fileList.Clear();
            fileButtonList.Clear();

            foreach (string str in files)
            {
                string[] temp = str.Split('/');
                fileList.Add(temp[temp.Length - 1]);
            }

            Transform[] transforms = content.GetComponentsInChildren<Transform>();

            for (int i = 1; i < transforms.Length; i++)
            {
                Destroy(transforms[i].gameObject);
            }

            for (int i = 0; i < fileList.Count; i++)
            {
                GameObject g = Instantiate(filePathPrefab, content.transform);
                g.GetComponentInChildren<Text>().text = fileList[i];

                fileButtonList.Add(g.GetComponent<Button>());
            }

            for (int i = 0; i < fileButtonList.Count; i++)
            {
                int closer = i;
                fileButtonList[i].onClick.AddListener(() =>
                {
                    filePathInputField.text = fileButtonList[closer].GetComponentInChildren<Text>().text;
                });
            }

            fileUIPanel.SetActive(true);
        }
    }

    public string GetFileName()
    {
        return filePathInputField.text;
    }

    public void FileUIActive(bool b)
    {
        fileUIPanel.SetActive(b);
    }
}
