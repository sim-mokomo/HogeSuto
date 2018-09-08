using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TurnCtrl : MonoBehaviour
{
    [SerializeField] private GameObject nextTurnHudRoot;

    public void Init()
    {
        nextTurnHudRoot.transform.position = new Vector3(1200.0f, 0.0f, 0.0f);
    }

    public void ShowNextTurnHud(bool show)
    {
        gameObject.SetActive(show);
    }

    public void ShowSequenceTurnHud()
    {
        nextTurnHudRoot.transform.DOMoveX(endValue: 0.0f, duration: 1.0f);
        
    }
}