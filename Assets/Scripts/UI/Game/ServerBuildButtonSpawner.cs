using System;
using Serverfull.Game;
using UnityEngine;
using Serverfull.Models;
using Zenject;

namespace Serverfull.UI.Game {
	public class ServerBuildButtonSpawner : MonoBehaviour {
		public ServerBuildButton SampleButton;

		ServerBuilder _builder;

		[Inject]
		public void Init(ServerBuilder builder) {
			_builder = builder;
		}
		
		void Start() {
			foreach ( ServerType type in Enum.GetValues(typeof(ServerType)) ) {
				var instance = Instantiate(SampleButton, SampleButton.transform.parent);
				instance.Init(_builder, type);
			}
			SampleButton.gameObject.SetActive(false);
		}
	}
}
