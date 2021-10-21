using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class NoteVO // Note 클래스는 이미 MonoBehaviour를 상속 받고 PoolManager에서 사용이 되고 있기에 따로 VO를 만들었다
{
    public float angle;
    public float time;
    public NoteManager.NoteEnum noteEnum;
}
