using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject rootObject;
    public GameObject circle;
    public GameObject judgeLine;

    public Text timeText;

    public float currentTime = 0;

    public float waitTime = 2;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("GameManager가 여러개 생성되었습니다");
        }
        instance = this;
    }

    void Update()
    {
        if (waitTime > 0f)
        {
            waitTime -= Time.deltaTime;
            return;
        }
        currentTime += Time.deltaTime;
        timeText.text = "Time : " + currentTime.ToString("0.00000");
    }
}
