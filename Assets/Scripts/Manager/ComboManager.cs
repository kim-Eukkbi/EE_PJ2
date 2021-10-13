using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    public Text comboText;

    private static ComboManager instance;

    private void Awake()
    {
        instance = this;
    }

    private int combo = 0;

    public static void ComboUp()
    {
        instance.combo++;
        instance.comboText.text = "Combo : " + instance.combo.ToString();
    }

    public static void ComboReset()
    {
        instance.combo = 0;
        instance.comboText.text = "Combo : " + instance.combo.ToString();
    }
}
