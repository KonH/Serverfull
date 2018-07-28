﻿using UnityEngine;
using Serverfull.Models;

namespace Serverfull.Views {
	public class ServerView : MonoBehaviour {
		public Transform  Center;
		public GameObject Selection;

		public ServerId Id { get; private set; }

		public void Init(ServerId id) {
			Id = id;
		}

		public void SetSelected(bool value) {
			Selection.SetActive(value);
		}
	}
}
