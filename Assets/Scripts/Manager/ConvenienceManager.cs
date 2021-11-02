using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvenienceManager : MonoBehaviour
{
    private float circleAngle;

    // Update is called once per frame
    void Update()
    {
        circleAngle = GameManager.instance.circle.transform.rotation.eulerAngles.z;
        if(circleAngle < 0)
        {
            circleAngle += 360;
        }

        if(InputManager.instance.NoteSpawnKeyDown)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            float dis = Vector3.Distance(mousePos, GameManager.instance.circle.transform.position);

            OptionManager.instance.GetCreateNote(circleAngle, AudioManager.instance.musicLength * AudioManager.instance.scrollbar.value + (dis / NoteManager.instance.noteSpeed), NoteManager.NoteEnum.Single);
            OptionManager.instance.UISetActive(2);
        }
    }
}
