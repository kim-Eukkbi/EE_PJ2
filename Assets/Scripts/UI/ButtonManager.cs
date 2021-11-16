using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class ButtonManager : MonoBehaviour
{
    public List<Button> buttons;

    private void Start()
    {
        foreach(var item in GetComponentsInChildren<Button>().ToList())
        {
            buttons.Add(item);
        }
       
        SetUI();

        buttons[0].onClick.AddListener(() =>
        {
            SceneManager.LoadScene("MusicSelect");
        });

        buttons[1].onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    private void SetUI()
    {
        transform.DOMoveX(-10f, 1);
    }
}
