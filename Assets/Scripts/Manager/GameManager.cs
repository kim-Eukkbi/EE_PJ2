using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject Circle;
    
    public float currentTime = 0;

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
        currentTime += Time.deltaTime;
    }
}
