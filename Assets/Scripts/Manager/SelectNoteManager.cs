using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectNoteManager : MonoBehaviour
{
    public static SelectNoteManager instance;

    private Note selectNote = null;

    public Note SelectNote
    {
        get
        {
            return selectNote;
        }
        set
        {
            if (selectNote == null)
            {
                selectNote = value;
            }
            else
            {
                if(value == null)
                {
                    selectNote = value;
                    return;
                }
                selectNote.angle = value.angle;
                selectNote.time = value.time;
                selectNote.noteEnum = value.noteEnum;
            }

            NoteManager.instance.SetNoteValue(selectNote);
        }
    }

    public LayerMask whatIsDCNote;
    public LayerMask whatIsSingleNote;
    public LayerMask whatIsLongNote;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("SelectNoteManager가 여러개 생성되었습니다");
        }
        instance = this;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D ray = Physics2D.Raycast(mousePos, Camera.main.transform.forward * -1, 20, whatIsDCNote | whatIsSingleNote | whatIsLongNote);
            
            if(ray.collider != null)
            {
                selectNote = ray.transform.GetComponent<Note>();

                OptionManager.instance.SetNoteInformationPanels(selectNote);
            }
        }
    }
}
