using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionButtonManager : MonoBehaviour
{
    public Button startBtn;
    public Button stopBtn;

    void Start()
    {
        startBtn.onClick.AddListener(() =>
        {
            GameManager.instance.GameStart();
        });

        stopBtn.onClick.AddListener(() =>
        {
            GameManager.instance.GameReset();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
