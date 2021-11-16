using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    public Button gameStartButton;

    private void Start()
    {
        gameStartButton.onClick.AddListener(() =>
        {
            RecvManager.instance.stageName = "R";
            SceneManager.LoadScene("InGame");
        });
    }
}
