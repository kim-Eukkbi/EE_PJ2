using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class ButtonManager : MonoBehaviour
{
    public List<Button> buttons;

    private void Start()
    {
        foreach(var item in GetComponentsInChildren<Button>().ToList())
        {
            buttons.Add(item);
        }

        SetUI();
    }

    private void SetUI()
    {
        Sequence UIseq = DOTween.Sequence();
        float j = 0;
        for (int i =0; i < buttons.Count;i++)
        {
            UIseq.Join(buttons[i].transform.DOMoveX(1,.5f + j));
            j += .25f;  
        }
    }
}
