using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BattleUICtrl : MonoBehaviour
{
	
	[SerializeField] private GameObject nextTurnHud;
	
	public void Init()
	{
		
	}

	public void Move()
	{
		
	}

	public void ShowNextTurnHud(bool show)
	{
		float distPos = 0.0f;
		distPos = show ? 100.0f : 0.0f;
		nextTurnHud.transform.DOMoveX(endValue: distPos,duration: 1.0f);
	}
	
}
