using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    public Text comboText;

    public static ComboManager instance;

    private void Awake()
    {
        instance = this;
    }

    private int combo = 0;

    // ?޺? 1 ????
    public void ComboUp()
    {
        instance.combo++;
        instance.comboText.text = "Combo : " + instance.combo.ToString();
    }

    // ?޺? 0???? ?ʱ?ȭ
    public void ComboReset()
    {
        instance.combo = 0;
        instance.comboText.text = "Combo : " + instance.combo.ToString();
    }
}
