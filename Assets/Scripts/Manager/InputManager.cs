using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public bool IsKeyDown { get; private set; }
    public bool IsKey { get; private set; }
    public int KeyInputCount { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("여러개의 InputManager를 생성하였습니다");
        }
        instance = this;
    }

    private void Update()
    {
        IsKeyDown = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.V); // 잠시 편한 키로 커스텀 해놓았슴

        IsKey = Input.GetKey(KeyCode.D);// 잠시 편한 키로 커스텀 해놓았슴
    }
}
