using System;
using System.Collections.Generic;
using UnityEngine;

namespace Serverfull.Views {
	public class RequestView : MonoBehaviour {
		public Color          GoodMoodColor;
		public Color          BadMoodColor;
		public List<Renderer> MoodRenderers;
		public TrailRenderer  Trail;

		[NonSerialized]
		public Vector3 StartPos;
		[NonSerialized]
		public Vector3 EndPos;
	}
}
