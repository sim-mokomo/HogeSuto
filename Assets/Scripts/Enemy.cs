using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Enemy : MonoBehaviour,I_Damageable
{

	[SerializeField] private Slider _hpGauge;
	[SerializeField] private float _maxHp;

	private ReactiveProperty<float> _hp = new ReactiveProperty<float>();

	public float Hp
	{
		get { return _hp.Value; }
		private set
		{
			_hp.SetValueAndForceNotify(Mathf.Clamp(value,0,_maxHp));
			_hpGauge.value = value;
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
		Hp = _maxHp;
		_hpGauge.maxValue = _maxHp;
		_hpGauge.value = _maxHp;
		
		_hp.Subscribe(hp =>
		{
			if (hp <= 0)
			{
				Destroy(this.gameObject);
			}
		});
		
	}

	public void Move()
	{
		
	}
}
