using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCtrl : MonoBehaviour
{
	private FlickListener _flickListener;
	private MarbleCtrl _marbleCtrl;

	void Start ()
	{
		_flickListener = FindObjectOfType<FlickListener>();
		_marbleCtrl = FindObjectOfType<MarbleCtrl>();
		
		_flickListener.Init();
		_marbleCtrl.Init();
	}
	
	void Update () {
		_flickListener.Move();
		_marbleCtrl.Move();
	}
}
