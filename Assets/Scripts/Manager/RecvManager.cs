using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecvManager : MonoBehaviour
{
    public static RecvManager instance;

    public string stageName;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("RecvManager�� ������ �����Ǿ����ϴ�");
        }
        instance = this;
    }
}
