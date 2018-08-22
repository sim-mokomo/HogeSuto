using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour,I_Damageable
{

	[SerializeField] private Slider _hpGauge;
	[SerializeField] private float _maxHp;
	
	private float _hp;

	public float Hp
	{
		get { return _hp; }
		private set
		{
			_hp -= value;
			_hp = Mathf.Clamp(_hp, 0, _maxHp);
			_hpGauge.value = _hp;
		}
	}

	public bool IsDamageable
	{
		get { return true; }
	}

	public void ApplyDamage(DamageInfo damageInfo)
	{
		Hp -= damageInfo.DamageValue;
	}
	
	public void Init()
	{
		_hpGauge.maxValue = _maxHp;
		_hpGauge.value = _maxHp;
	}

	public void Move()
	{
		
	}
}
