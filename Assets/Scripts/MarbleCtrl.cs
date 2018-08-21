using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleCtrl : MonoBehaviour
{
	private Rigidbody2D _rigidbody2D;
	[SerializeField] private float _flickPower;
	[Range(0.0f,1.0f)] [SerializeField] private float _fliction;
	
	public void Init()
	{
		_rigidbody2D = GetComponent<Rigidbody2D>();
		FlickListener.OnEndFlick += flickData =>
		{
			Debug.Log(flickData.ToString());
			_rigidbody2D.velocity = flickData.FlickDirection * _flickPower;
		};
	}

	public void Move()
	{
		_rigidbody2D.velocity = _rigidbody2D.velocity * (1.0f -_fliction);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		Vector3 normal = other.contacts[0].normal;
		_rigidbody2D.velocity = Vector3.Reflect(_rigidbody2D.velocity, normal);
	}
}
