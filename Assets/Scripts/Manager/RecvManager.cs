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
            Debug.LogError("RecvManager가 여러개 생성되었습니다");
        }
        instance = this;
    }
}
