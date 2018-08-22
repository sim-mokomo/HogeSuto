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
	private List<Enemy> _enemyMarbleList = new List<Enemy>();
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


		_enemyMarbleList = FindObjectsOfType<Enemy>().ToList();
		foreach (var em in _enemyMarbleList)
		{
			em.Init();
		}
	}
	
	void Update () {
		_playerMarbleList.ForEach(marble => marble.Move());
		_enemyMarbleList.ForEach(em => em.Move());
	}
}
