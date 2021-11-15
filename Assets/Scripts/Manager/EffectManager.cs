using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    public ParticleSystem circleEffect;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("EffectManager�� ������ �����մϴ�");
        }
        instance = this;
    }

    public void SpawnCircle()
    {
        circleEffect.Play();
    }

}
