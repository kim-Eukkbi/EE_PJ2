using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRotation : MonoBehaviour
{
    public GameObject testObj;

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 dir = mousePos - transform.position;
        Debug.Log(Quaternion.FromToRotation(Vector2.right, dir).eulerAngles);
        transform.rotation = Quaternion.FromToRotation(Vector2.up, dir);


    }
}
