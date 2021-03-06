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
            Debug.LogError("EffectManager가 여러개 존재합니다");
        }
        instance = this;
    }

    public void SpawnCircle()
    {
        circleEffect.Play();
    }

}
