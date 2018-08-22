using System;
using System.Text;
using UnityEngine;

public class FlickData
{
	public FlickData(Vector3 startPos, Vector3 endPos)
	{
		FlickDirection = (endPos - startPos).normalized;
		StartPos = startPos;
		EndPos = endPos;
	}

	public Vector3 StartPos { get; }
	public Vector3 EndPos { get; }
	public Vector3 FlickDirection { get; }

	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();
		builder.AppendLine($"Start Flick Pos {StartPos}");
		builder.AppendLine($"End Flick Pos {EndPos}");
		builder.AppendLine($"Flick Direction {FlickDirection}");
		return builder.ToString();
	}
}

public class FlickListener : MonoBehaviour
{

	private Vector3 _startFlickPos;

	public Action<Vector3> OnStartFlick;
	public Action<FlickData> OnEndFlick;
	public Action<Vector3> OnFlicking;
	
	public void Init()
	{
		_startFlickPos = Vector3.zero;
		OnStartFlick = null;
		OnEndFlick = null;
		OnFlicking = null;
	}

	public void Move()
	{
		if (Input.GetMouseButtonDown(button: 0))
		{
			_startFlickPos = GetWorldMousePos();
			OnStartFlick?.Invoke(_startFlickPos);
		}
		else if (Input.GetMouseButtonUp(button: 0))
		{
			Vector3 endFlickPos = GetWorldMousePos();
			FlickData flickData = new FlickData(startPos: _startFlickPos,endPos: endFlickPos);
			OnEndFlick?.Invoke(flickData);
		}else if (Input.GetMouseButton(button: 0))
		{
			OnFlicking?.Invoke(GetWorldMousePos());
		}
	}

	private Vector3 GetWorldMousePos()
	{
		return Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
	
}
