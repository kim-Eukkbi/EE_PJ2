using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public bool IsKeyDown { get; private set; }
    public bool IsKey { get; private set; }
    public bool RemoveNoteKeyDown { get; private set; }
    public bool DCNoteSpawnKeyDown { get; private set; }
    public bool SingleNoteSpawnKeyDown { get; private set; }
    public bool LongNoteSpawnKeyDown { get; private set; }
    public bool isGameStartKeyDown { get; private set; }
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
        IsKeyDown = Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.V); // 잠시 편한 키로 커스텀 해놓았슴

        IsKey = Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.V);// 잠시 편한 키로 커스텀 해놓았슴

        SingleNoteSpawnKeyDown = Input.GetKeyDown(KeyCode.S);
        DCNoteSpawnKeyDown = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.X);
        LongNoteSpawnKeyDown = Input.GetKeyDown(KeyCode.D);
        RemoveNoteKeyDown = Input.GetKeyDown(KeyCode.R);
        isGameStartKeyDown = Input.GetKeyDown(KeyCode.Return);

    }
}
