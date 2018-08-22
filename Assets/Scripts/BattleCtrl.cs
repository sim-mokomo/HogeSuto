using UniRx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BattleCtrl : MonoBehaviour
{
	private List<MarbleCtrl> _playerMarbleList = new List<MarbleCtrl>();
	public Action OnTurnChange;
	
	void Start()
	{
		_playerMarbleList = FindObjectsOfType<MarbleCtrl>().ToList();
		foreach (var playerMarble in _playerMarbleList)
		{
			playerMarble.Init();
			playerMarble.OnStopFlickMove
				.ThrottleFirstFrame(frameCount: 120)
				.Subscribe(self =>
			{
				Debug.Log("Change Turn");
				self.Deactivate();
				int curIndex = _playerMarbleList.FindIndex(marble => marble == self);
				curIndex++;
				int distMarbleIndex = curIndex % _playerMarbleList.Count;
				_playerMarbleList[distMarbleIndex].Activate();
				
				OnTurnChange?.Invoke();
				foreach (var pl in _playerMarbleList)
				{
					pl.TurnInit();
				}
			});
		}
		_playerMarbleList.First().Activate();
	}
	
	void Update () {
		_playerMarbleList.ForEach(marble => marble.Move());
	}
}
