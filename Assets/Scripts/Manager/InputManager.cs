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
            Debug.LogError("�������� InputManager�� �����Ͽ����ϴ�");
        }
        instance = this;
    }

    private void Update()
    {
        IsKeyDown = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.V); // ��� ���� Ű�� Ŀ���� �س��ҽ�

        IsKey = Input.GetKey(KeyCode.D);// ��� ���� Ű�� Ŀ���� �س��ҽ�

        SingleNoteSpawnKeyDown = Input.GetKeyDown(KeyCode.S);
        DCNoteSpawnKeyDown = Input.GetKeyDown(KeyCode.A);
        LongNoteSpawnKeyDown = Input.GetKeyDown(KeyCode.D);
        RemoveNoteKeyDown = Input.GetKeyDown(KeyCode.R);
        isGameStartKeyDown = Input.GetKeyDown(KeyCode.Return);

    }
}
