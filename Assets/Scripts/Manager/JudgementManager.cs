using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgementManager : MonoBehaviour
{
    public GameObject TestObj;
    public LayerMask whatIsNote;

    private Vector3 judgementSize;

    private void Update()
    {
        judgementSize = new Vector3(1, 0.1f, 1) * 
                        GameManager.instance.rootObject.transform.localScale.x * 
                        GameManager.instance.circle.transform.localScale.x;

        OutNoteCheck();
        JudgementCheck();
    }

    private void OutNoteCheck()
    {
        Collider2D col = Physics2D.OverlapCircle(GameManager.instance.circle.transform.position,
                                          GameManager.instance.circle.transform.lossyScale.x * 0.5f,
                                          whatIsNote);

        if (col != null)
        {
            NoteManager.RemoveNote(col.gameObject);
        }
    }

    private void JudgementCheck()
    {
        Collider2D col = Physics2D.OverlapBox(GameManager.instance.judgeLine.transform.position,
                                                judgementSize,
                                                GameManager.instance.circle.transform.rotation.eulerAngles.z,
                                                whatIsNote);

        if(col != null)
        {
            Note note = col.GetComponent<Note>();

            switch(note.noteEnum)
            {
                case NoteManager.NoteEnum.DC:

                    break;
                case NoteManager.NoteEnum.Single:

                    break;
                case NoteManager.NoteEnum.Long:

                    break;
            }

            NoteManager.RemoveNote(col.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GameManager.instance.circle.transform.position, GameManager.instance.circle.transform.lossyScale.x * 0.5f);

        Vector2 pos = GameManager.instance.judgeLine.transform.position;

        // Gizmos의 DrawWireCube는 회전 관련 변수가 없어서 DrawWireMesh 함수를 사용했어요
        // 그냥 네모를 만들기 위해서 삼각형 두개를 생성한거에요
        Mesh m = new Mesh(); // 삼각형 1
        Mesh m2 = new Mesh(); // 삼각형 2

        m.vertices = new Vector3[] { new Vector3(-.5f, -.5f, 0f), new Vector3(.5f, -.5f, 0f), new Vector3(.5f, .5f, 0f) }; 
        m.triangles = new int[] { 0, 1, 2 };
        m.normals = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward };

        m2.vertices = new Vector3[] { new Vector3(-.5f, .5f, 0f), new Vector3(-.5f, -.5f, 0f), new Vector3(.5f, .5f, 0f) };
        m2.triangles = new int[] { 0, 1, 2 };
        m2.normals = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward };

        Gizmos.color = Color.blue;

        Gizmos.DrawWireMesh(m, pos, GameManager.instance.circle.transform.rotation, judgementSize);
        Gizmos.DrawWireMesh(m2, pos, GameManager.instance.circle.transform.rotation, judgementSize);
    }
}
