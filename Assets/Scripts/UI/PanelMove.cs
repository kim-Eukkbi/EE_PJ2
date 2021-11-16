using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PanelMove : MonoBehaviour
{
    public Button moveButton;

    // Start is called before the first frame update
    void Start()
    {
        moveButton.onClick.AddListener(() =>
        {
            transform.DOLocalMoveY(-1080, 1).SetEase(Ease.InQuad);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
