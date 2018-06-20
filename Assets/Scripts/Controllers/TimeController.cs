using UnityEngine;
using Serverfull.Game;
using Zenject;

namespace Serverfull.Controllers {
	public class TimeController : ITickable {
		public float DeltaTime { get; private set; }

		readonly GameSettings _settings;

		public TimeController(GameSettings settings) {
			_settings = settings;
		}

		public void Tick() {
			DeltaTime = Time.deltaTime * _settings.TimeScale;
		}
	}
}
