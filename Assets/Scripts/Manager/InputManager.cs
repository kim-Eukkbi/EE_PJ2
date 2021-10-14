using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public bool IsSpaceDown { get; set; }
    public bool IsSpace { get; set; }

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
        IsSpaceDown = Input.GetKeyDown(KeyCode.D);
        IsSpace = Input.GetKey(KeyCode.D);
    }
}
