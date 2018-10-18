using System;
using UnityEngine;
using UDBase.Controllers.LogSystem;
using UDBase.Controllers.SaveSystem;
using UDBase.Controllers.EventSystem;
using Serverfull.Common;
using Serverfull.Models;
using Zenject;

namespace Serverfull.Controllers {
	public class TimeController : ITickable, ILogContext, ISavable {
		public TimeModel State     { get; private set; }
		public DateTime  GameTime  => State.Time;
		public float     DeltaTime { get; private set; }

		readonly GameSettings _settings;

		bool _paused;

		public TimeController(GameSettings settings) {
			_settings = settings;
		}

		public void Load(ISave save) {
			State = save.GetNode<TimeModel>(false);
			if ( State == null ) {
				State = new TimeModel(DateTime.MinValue, DateTime.MinValue);
			}
		}

		public void Save(ISave save) {
			save.SaveNode(State);
		}

		public void Tick() {
			DeltaTime = _paused ? 0 : Time.deltaTime * _settings.TimeScale;
			State.Time = GameTime.AddSeconds(DeltaTime);
		}

		public void Pause() {
			_paused = true;
			Time.timeScale = 0.0f;
		}

		public void Resume() {
			_paused = false;
			Time.timeScale = 1.0f;
		}
	}
}
