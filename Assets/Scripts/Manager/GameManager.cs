using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject rootObject;
    public GameObject circle;
    public GameObject judgeLine;
    
    public float currentTime = 0;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("GameManager�� ������ �����Ǿ����ϴ�");
        }
        instance = this;
    }

    void Update()
    {
        currentTime += Time.deltaTime;
    }
}
