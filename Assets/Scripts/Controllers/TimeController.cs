using System;
using UnityEngine;
using UDBase.Controllers.LogSystem;
using UDBase.Controllers.EventSystem;
using Serverfull.Game;
using Serverfull.Events;
using Zenject;

namespace Serverfull.Controllers {
	public class TimeController : ITickable, ILogContext {
		public DateTime GameTime  { get; private set; }
		public float    DeltaTime { get; private set; }

		readonly ULogger      _log;
		readonly IEvent       _event;
		readonly GameSettings _settings;

		DateTime _startTime;
		bool     _firstTicked;
		bool     _paused;
		int      _prevHours;

		public TimeController(ILog log, IEvent events, GameSettings settings) {
			_log       = log.CreateLogger(this);
			_event     = events;
			_settings  = settings;
			_startTime = DateTime.MinValue;
			GameTime   = _startTime;
		}

		public void Tick() {
			DeltaTime = _paused ? 0 : Time.deltaTime * _settings.TimeScale;
			GameTime = GameTime.AddSeconds(DeltaTime);
			if ( !_firstTicked ) {
				_event.Fire(new Time_Started());
				_firstTicked = true;
			}
			var hoursDelta = (int)(GameTime - _startTime).TotalHours;
			while ( hoursDelta > _prevHours ) {
				_log.MessageFormat("New game hour: {0}", GameTime);
				_event.Fire(new Time_NewGameHour(GameTime));
				_prevHours++;
			}
		}

		public void Pause() {
			_paused = true;
		}

		public void Resume() {
			_paused = false;
		}
	}
}
