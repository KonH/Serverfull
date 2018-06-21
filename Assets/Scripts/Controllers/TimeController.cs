using System;
using UnityEngine;
using UDBase.Controllers.LogSystem;
using Serverfull.Game;
using Serverfull.Events;
using Zenject;
using UDBase.Controllers.EventSystem;

namespace Serverfull.Controllers {
	public class TimeController : ITickable, ILogContext {
		public DateTime GameTime  { get; private set; }
		public float    DeltaTime { get; private set; }

		readonly ULogger      _log;
		readonly IEvent       _event;
		readonly GameSettings _settings;

		DateTime _startTime;
		int      _prevHours;

		public TimeController(ILog log, IEvent events, GameSettings settings) {
			_log       = log.CreateLogger(this);
			_event     = events;
			_settings  = settings;
			_startTime = DateTime.MinValue;
			GameTime   = _startTime;
		}

		public void Tick() {
			DeltaTime = Time.deltaTime * _settings.TimeScale;
			GameTime = GameTime.AddSeconds(DeltaTime);
			var hoursDelta = (int)(GameTime - _startTime).TotalHours;
			while ( hoursDelta > _prevHours ) {
				_log.MessageFormat("New game hour: {0}", GameTime);
				_event.Fire(new Time_NewGameHour(GameTime));
				_prevHours++;
			}
		}
	}
}
