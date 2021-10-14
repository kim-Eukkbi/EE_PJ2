using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class JudgementManager : MonoBehaviour
{
    public LayerMask whatIsDCNote;
    public LayerMask whatIsSingleNote;
    public LayerMask whatIsLongNote;

    public float perfectSize = 1;
    public float greatSize = 2;
    public float missSize = 4;

    private Vector3 perfectJudgementSize;
    private Vector3 greatJudgementSize;
    private Vector3 missJudgementSize;

    private void Update()
    {
        // perfect 판정 범위
        perfectJudgementSize = new Vector3(1, perfectSize * NoteManager.instance.noteSpeed, 1) * 
                        GameManager.instance.rootObject.transform.localScale.x * 
                        GameManager.instance.circle.transform.localScale.x;
        // great 판정 범위
        greatJudgementSize = new Vector3(1, greatSize * NoteManager.instance.noteSpeed, 1) *
                        GameManager.instance.rootObject.transform.localScale.x *
                        GameManager.instance.circle.transform.localScale.x;
        // miss 판정 범위
        missJudgementSize = new Vector3(1, missSize, 1) *
                        GameManager.instance.rootObject.transform.localScale.x *
                        GameManager.instance.circle.transform.localScale.x;

        OutNoteCheck();

        // 1프레임 당 한개의 노트만 체크하기
        // 우선 순위 : single > long > dc
        if (NoteJudgementCheck(InputManager.instance.IsSpaceDown,
                                InputManager.instance.IsSpaceDown, 
                                InputManager.instance.IsSpaceDown, 
                                whatIsSingleNote))
        {
            return;
        }
        else if (NoteJudgementCheck((InputManager.instance.IsSpace || InputManager.instance.IsSpaceDown), 
                                    InputManager.instance.IsSpaceDown, 
                                    InputManager.instance.IsSpaceDown, 
                                    whatIsLongNote))
        {
            return;
        }
        NoteJudgementCheck(true, false, false, whatIsDCNote);
    }

    // 노트를 놓쳤을 때
    private void OutNoteCheck() 
    {
        Collider2D col = Physics2D.OverlapCircle(GameManager.instance.circle.transform.position,
                                          GameManager.instance.circle.transform.lossyScale.x * 0.5f,
                                          whatIsSingleNote | whatIsDCNote | whatIsLongNote);

        if (col != null)
        {
            Miss(col.gameObject.GetComponent<Note>());
        }
    }

    float currentTime = 0;

    // perfect, great, miss 판정 검사 함수
    private bool NoteJudgementCheck(bool perfect, bool great, bool miss, LayerMask noteEnum) 
    {
        Collider2D perfectCol = Physics2D.OverlapBox(GameManager.instance.judgeLine.transform.position,
                                                perfectJudgementSize,
                                                GameManager.instance.circle.transform.rotation.eulerAngles.z,
                                                noteEnum);

        Collider2D greatCol = Physics2D.OverlapBox(GameManager.instance.judgeLine.transform.position,
                                                greatJudgementSize,
                                                GameManager.instance.circle.transform.rotation.eulerAngles.z,
                                                noteEnum);

        Collider2D missCol = Physics2D.OverlapBox(GameManager.instance.judgeLine.transform.position,
                                                missJudgementSize,
                                                GameManager.instance.circle.transform.rotation.eulerAngles.z,
                                                noteEnum);

        if (perfectCol != null && perfect)
        {
            Note note = perfectCol.gameObject.GetComponent<Note>();
            Perfect(note);

            return true;
        }
        else if (greatCol != null && great)
        {
            Note note = greatCol.gameObject.GetComponent<Note>();

            Great(note);
            return true;
        }
        else if (missCol != null && miss)
        {
            Note note = missCol.gameObject.GetComponent<Note>();

            Miss(note);
            return true;
        }

        return false;
    }

    private void Perfect(Note note)
    {
        NoteManager.RemoveNote(note);
        ComboManager.ComboUp();
    }

    private void Great(Note note)
    {
        NoteManager.RemoveNote(note);
        ComboManager.ComboUp();
    }

    private void Miss(Note note)
    {
        NoteManager.RemoveNote(note);
        ComboManager.ComboReset();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(GameManager.instance.circle.transform.position, GameManager.instance.circle.transform.lossyScale.x * 0.5f);

        Vector2 perfectPos = GameManager.instance.judgeLine.transform.position;
        Vector2 greatPos = GameManager.instance.judgeLine.transform.position;
        Vector2 missPos = GameManager.instance.judgeLine.transform.position;

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

        DrawBox(m, m2, missPos, missJudgementSize, Color.red);
        DrawBox(m, m2, greatPos, greatJudgementSize, Color.green);
        DrawBox(m, m2, perfectPos, perfectJudgementSize, Color.blue);
    }

    private void DrawBox(Mesh m1, Mesh m2, Vector3 pos, Vector3 size, Color color)
    {
        Gizmos.color = color;

        Gizmos.DrawWireMesh(m1, pos, GameManager.instance.circle.transform.rotation, size);
        Gizmos.DrawWireMesh(m2, pos, GameManager.instance.circle.transform.rotation, size);
    }
}

// 나중에 쓸 코드 저장소
//currentTime += Time.deltaTime;

//float dis = Vector3.Distance(greatCol.transform.position, GameManager.instance.judgeLine.transform.position);

//if (Mathf.Abs(dis) < 0.05f)
//{
//    Debug.Log("currentTime : " + currentTime + "ms");
//    currentTime = 0;

//    Note note2 = greatCol.gameObject.GetComponent<Note>();

//    Perfect(note2);
//}