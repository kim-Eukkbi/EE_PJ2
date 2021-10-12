using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgementManager : MonoBehaviour
{

    private void Update()
    {
        Debug.Log("Scale : " + GameManager.instance.Circle.transform.lossyScale);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Vector3.zero, GameManager.instance.Circle.transform.lossyScale.x * 0.5f);
    }
}
