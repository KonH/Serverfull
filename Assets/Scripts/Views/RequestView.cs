using System;
using UnityEngine;

public class RequestView : MonoBehaviour {
	public Color GoodMoodColor;
	public Color BadMoodColor;

	public Renderer MoodRenderer;

	[NonSerialized]
	public Vector3 StartPos;
	[NonSerialized]
	public Vector3 EndPos;
}
