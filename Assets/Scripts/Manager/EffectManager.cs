using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("EffectManager�� ������ �����մϴ�");
        }
        instance = this;
    }



}
