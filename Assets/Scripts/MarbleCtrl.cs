﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleCtrl : MonoBehaviour
{
	private Rigidbody2D _rigidbody2D;
	[SerializeField] private float _flickPower;
	[Range(0.0f,1.0f)] [SerializeField] private float _fliction;

	[SerializeField] private GameObject flickArrowRoot;
	[SerializeField] private GameObject flickArrow;
	[SerializeField] private float flickScalingMaxBounds;
	[SerializeField] private float flickMinSize;
	[SerializeField] private float flickMaxSize;
	
	public void Init()
	{
		_rigidbody2D = GetComponent<Rigidbody2D>();
		FlickListener.OnEndFlick += flickData =>
		{
			Debug.Log(flickData.ToString());
			_rigidbody2D.velocity = flickData.FlickDirection * -_flickPower;
			flickArrow.transform.localScale = Vector3.one;
		};

		FlickListener.OnFlicking += flickingPos =>
		{
			Vector3 diff = flickingPos - transform.position;
			float angle = Mathf.Atan2(diff.y,diff.x) * Mathf.Rad2Deg;
			angle += 90.0f;
			flickArrowRoot.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle));

			float distToMouse = (flickingPos - transform.position).sqrMagnitude;
			distToMouse = distToMouse - 100.0f;
			float distToMouseRatio = distToMouse / flickScalingMaxBounds;
			
			Debug.Log($"dist to mouse {distToMouse}");
			Debug.Log($"dist to mouse distance ratio {distToMouseRatio}");
			
			float scalingSize = Mathf.Lerp(flickMinSize, flickMaxSize, distToMouseRatio);
			Vector3 scale = Vector3.one;
			scale.y = scalingSize;
			flickArrow.transform.localScale = scale;
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
