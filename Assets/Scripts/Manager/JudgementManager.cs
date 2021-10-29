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

    public float perfectJudgement = 0.50f;
    public float greatJudgement = 0.105f;
    public float missJudgement = 2;

    private Vector3 perfectJudgementSize;
    private Vector3 greatJudgementSize;
    private Vector3 missJudgementSize;

    private void Update()
    {
        if (!GameManager.instance.isGameStart) return;

        perfectJudgementSize = new Vector3(GameManager.instance.judgeLine.transform.localScale.x, perfectJudgement * NoteManager.instance.noteSpeed, 1) *
                        GameManager.instance.rootObject.transform.localScale.x *
                        GameManager.instance.circle.transform.localScale.x;
        greatJudgementSize = new Vector3(GameManager.instance.judgeLine.transform.localScale.x, greatJudgement * NoteManager.instance.noteSpeed, 1) *
                        GameManager.instance.rootObject.transform.localScale.x *
                        GameManager.instance.circle.transform.localScale.x;
        missJudgementSize = new Vector3(GameManager.instance.judgeLine.transform.localScale.x, missJudgement * NoteManager.instance.noteSpeed, 1) *
                        GameManager.instance.rootObject.transform.localScale.x *
                        GameManager.instance.circle.transform.localScale.x;

        OutNoteCheck();

        FrameNoteCheck();
    }

    // 1 ������ ���� ��Ʈ�� ������ Ȯ���ϴ� �Լ�
    private void FrameNoteCheck()
    {
        if (NoteJudgementCheck(InputManager.instance.IsKeyDown,
                InputManager.instance.IsKeyDown,
                InputManager.instance.IsKeyDown,
                whatIsSingleNote | whatIsLongNote)){
        }else if (NoteJudgementCheck(true, false, false, whatIsDCNote))
        {
            Debug.Log("�̰ǰ�??");
            NoteJudgementCheck(true, false, false, whatIsDCNote);
        }
    }

    // ��Ʈ�� ������ ��
    private void OutNoteCheck()
    {
        Collider2D col = Physics2D.OverlapCircle(GameManager.instance.circle.transform.position,
                                          GameManager.instance.circle.transform.lossyScale.x * 0.5f,
                                          whatIsSingleNote | whatIsDCNote | whatIsLongNote);

        if (col != null)
        {
            Note note = col.gameObject.GetComponent<Note>();
            Miss(note);

            // �� �����Ӵ� �˻��ص� ������ �� ������ ������ �˻�
            OutNoteCheck();
        }
    }

    private bool NoteJudgementCheck(bool perfect, bool great, bool miss, LayerMask noteEnum)
    {
        Collider2D[] judgeCols = Physics2D.OverlapBoxAll(GameManager.instance.judgeLine.transform.position + (GameManager.instance.circle.transform.up * (missJudgement * (NoteManager.instance.noteSpeed / 2.35f))),
                    missJudgementSize,
                    GameManager.instance.circle.transform.rotation.eulerAngles.z,
                    noteEnum);

        if (judgeCols.Length == 0) return false;

        Array.Sort(judgeCols, (x, y) => x.GetComponent<Note>().time.CompareTo(y.GetComponent<Note>().time));

        Note note = judgeCols[0].GetComponent<Note>();

        if (note.noteEnum == NoteManager.NoteEnum.Long)
        {
            perfect = InputManager.instance.IsKey || InputManager.instance.IsKeyDown;
            Debug.Log("LONG NOTE IS HEREEEEEEEEEEE");
        }

        float judgementTime = Mathf.Abs(GameManager.instance.currentTime - note.time);

        if (perfect && judgementTime < perfectJudgement)
        {
            Perfect(note);

            //Debug.Log(judgementTime);
            //Debug.Log(note.time + " - " + GameManager.instance.currentTime);

            return true;
        }
        else if (great && judgementTime < greatJudgement)
        {
            Great(note);

            //Debug.Log(judgementTime + " - Great");
            //Debug.Log(note.time + " - " + GameManager.instance.currentTime);

            return true;
        }
        else if (miss && judgementTime < missJudgement)
        {
            Miss(note);

            //Debug.Log(judgementTime + " - Miss");
            //Debug.Log(note.time + " - " + GameManager.instance.currentTime);

            return true;
        }

        return false;
    }

    private void Perfect(Note note)
    {
        NoteManager.instance.RemoveNote(note);
        ComboManager.instance.ComboUp();
    }

    private void Great(Note note)
    {
        NoteManager.instance.RemoveNote(note);
        ComboManager.instance.ComboUp();
    }

    private void Miss(Note note)
    {
        NoteManager.instance.RemoveNote(note);
        ComboManager.instance.ComboReset();
        //ComboManager.ComboUp();
    }

    private void OnDrawGizmos()
    {
        // NullReferenceException ������ ���ٸ� �װ� ����Ƽ �������� �ſ� �����Դϴ�
        Gizmos.color = Color.black;
        if(GameManager.instance != null && GameManager.instance.circle != null)
        {
            Gizmos.DrawWireSphere(GameManager.instance.circle.transform.position, GameManager.instance.circle.transform.lossyScale.x * 0.5f);

            Vector2 perfectPos = GameManager.instance.judgeLine.transform.position;
            Vector2 greatPos = GameManager.instance.judgeLine.transform.position;
            Vector2 missPos = GameManager.instance.judgeLine.transform.position;

            // Gizmos�� DrawWireCube�� ȸ�� ���� ������ ��� DrawWireMesh �Լ��� ����߾��
            // �׳� �׸� ����� ���ؼ� �ﰢ�� �ΰ��� �����Ѱſ���
            Mesh m = new Mesh(); // �ﰢ�� 1
            Mesh m2 = new Mesh(); // �ﰢ�� 2

            m.vertices = new Vector3[] { new Vector3(-.5f, -.5f, 0f), new Vector3(.5f, -.5f, 0f), new Vector3(.5f, .5f, 0f) };
            m.triangles = new int[] { 0, 1, 2 };
            m.normals = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward };

            m2.vertices = new Vector3[] { new Vector3(-.5f, .5f, 0f), new Vector3(-.5f, -.5f, 0f), new Vector3(.5f, .5f, 0f) };
            m2.triangles = new int[] { 0, 1, 2 };
            m2.normals = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward };

            DrawBox(m, m2, missPos + (Vector2)(GameManager.instance.circle.transform.up * (missJudgement * (NoteManager.instance.noteSpeed / 2.35f))), missJudgementSize, Color.red);
            DrawBox(m, m2, greatPos, greatJudgementSize, Color.green);
            DrawBox(m, m2, perfectPos, perfectJudgementSize, Color.blue);
        }
    }

    private void DrawBox(Mesh m1, Mesh m2, Vector3 pos, Vector3 size, Color color)
    {
        Gizmos.color = color;

        Gizmos.DrawWireMesh(m1, pos, GameManager.instance.circle.transform.rotation, size);
        Gizmos.DrawWireMesh(m2, pos, GameManager.instance.circle.transform.rotation, size);
    }
}