using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConvenienceManager : MonoBehaviour
{
    public static ConvenienceManager instance;

    public LayerMask whatIsDCNote; // DCNote �� layer
    public LayerMask whatIsSingleNote;// SingleNote �� layer
    public LayerMask whatIsLongNote;// LongNote �� layer

    public Toggle SpawnNotesToggle; // �÷��� ���� a, s, d Ű�� ���ؼ� ��Ʈ�� ���� (true, false)

    private float circleAngle; // ���� ���� 
    private List<Note> startingNotes = new List<Note>(); // start �� ���� ���� �Ҷ� ��� �ӽ÷� ��Ʈ���� ����

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("�������� ConvenienceManager�� �����Ǿ����ϴ�");
        }
        instance = this;
    }
    void Update()
    {
        circleAngle = GameManager.instance.circle.transform.rotation.eulerAngles.z;

        if (circleAngle < 0) // ���� ó�� ����
        {
            circleAngle += 360;
        }

        if (!GameManager.instance.isGameStart) // ���� ������ ��忡�� �÷��� ������ �ƴ� ��
        {
            if (InputManager.instance.SingleNoteSpawnKeyDown)
            {
                SpawnNoteMousePos(NoteManager.NoteEnum.Single);
            }
            else if(InputManager.instance.DCNoteSpawnKeyDown)
            {
                SpawnNoteMousePos(NoteManager.NoteEnum.DC);
            }
            else if (InputManager.instance.LongNoteSpawnKeyDown)
            {
                SpawnNoteMousePos(NoteManager.NoteEnum.Long);
            }

            // ���� Ű�� ������ ���콺 ��ġ�� �ִ� ��Ʈ�� ���� �Ѵ�
            if(InputManager.instance.RemoveNoteKeyDown) 
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D ray = Physics2D.Raycast(mousePos, Camera.main.transform.forward * -1, 20, whatIsDCNote | whatIsSingleNote | whatIsLongNote);

                if (ray.collider != null)
                {   
                    NoteManager.instance.RemoveNote(ray.transform.GetComponent<Note>());
                }
            }
        }
        else // ������ ��忡�� ���� �÷��� ���� �϶�
        {
            if(SpawnNotesToggle.isOn)
            {
                // ��Ʈ ���� Ű�� �ƹ��ų� ������ �� �ش��ϴ� ��Ʈ�� �����ϰ� �׿� ���� ���� ���� �� ��� ��Ȱ��ȭ
                if(InputManager.instance.DCNoteSpawnKeyDown ||
                    InputManager.instance.SingleNoteSpawnKeyDown ||
                    InputManager.instance.LongNoteSpawnKeyDown)
                {
                    Note note = null;

                    if (InputManager.instance.SingleNoteSpawnKeyDown)
                    {
                        note = PoolManager.GetCreateItem<SingleNote>(NoteManager.instance.singleNotePrefab);
                    }
                    else if(InputManager.instance.DCNoteSpawnKeyDown)
                    {
                        note = PoolManager.GetCreateItem<DCNote>(NoteManager.instance.dcNotePrefab);
                    }
                    else if (InputManager.instance.LongNoteSpawnKeyDown)
                    {
                        note = PoolManager.GetCreateItem<LongNote>(NoteManager.instance.longNotePrefab);
                    }

                    note.gameObject.SetActive(false);

                    note.time = GameManager.instance.currentTime;
                    note.angle = circleAngle;

                    startingNotes.Add(note);
                }
            }
        }
    }

    // stop�� �� �� ��Ʈ�� �ٽ� ����
    public void SetStartingNotes()
    {
        for(int i = 0; i < startingNotes.Count; i++)
        {
            NoteManager.instance.AddNote(startingNotes[i]);
            NoteManager.instance.SetNoteValue(startingNotes[i]);
        }

        startingNotes.Clear();
    }

    // ���콺 ��ġ�� �Ű������� �´� ��Ʈ ����
    private void SpawnNoteMousePos(NoteManager.NoteEnum noteEnum)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        float dis = Vector3.Distance(mousePos, GameManager.instance.circle.transform.position)
                                    - GameManager.instance.judgeLineY
                                    - GameManager.instance.judgeLine.transform.localScale.y;

        OptionManager.instance.GetCreateNote(circleAngle,
                                    AudioManager.instance.musicLength * AudioManager.instance.scrollbar.value + (dis / NoteManager.instance.noteSpeed),
                                    noteEnum);
        OptionManager.instance.UISetActive(2);
    }
}
