using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UDBase.Utils;
using Serverfull.Models;
using Serverfull.Controllers;
using Zenject;

namespace Serverfull.UI.Game {
	public class ServerPanel : MonoBehaviour {
		const float UpdateTime = 0.33f;
		
		[Serializable]
		public class ServerResourcePanel {
			public string Name;
			public Slider Slider;
		}

		public int                       ServerIndex;
		public GameObject                Root;
		public List<ServerResourcePanel> Resources;

		ServerController _server;
		float            _timer;

		[Inject]
		public void Init(ServerController server) {
			_server = server;
		}

		void Start() {
			UpdateState();
		}

		void Update() {
			UpdateState();
		}

		void UpdateState() {
			var server = _server.Get(new ServerId(ServerIndex));
			Root.SetActive(server != null);
			if ( server != null ) {
				_timer += Time.deltaTime;
				if ( _timer < UpdateTime ) {
					return;
				}
				_timer = 0.0f;
				foreach ( var res in Resources ) {
					var value = server.Resources.GetOrDefault(res.Name);
					res.Slider.value = value != null ? value.NormalizedFree : 0.0f;
				}
			} else {
				_timer = UpdateTime;
			}
		}
	}
}
