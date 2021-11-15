using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRotation : MonoBehaviour
{
    // ���콺 ��ġ�� ���� ���� ������
    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 dir = mousePos - transform.position;

        transform.rotation = Quaternion.FromToRotation(Vector2.up, dir);
    }
}
