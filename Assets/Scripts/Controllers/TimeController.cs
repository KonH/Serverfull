using System;
using UnityEngine;
using UDBase.Controllers.LogSystem;
using UDBase.Controllers.SaveSystem;
using UDBase.Controllers.EventSystem;
using Serverfull.Common;
using Serverfull.Events;
using Zenject;

namespace Serverfull.Controllers {
	public class TimeController : ITickable, ILogContext {
		public class State : ISaveSource {
			public DateTime Time;
		}

		public DateTime GameTime => _state.Time;
		public float    DeltaTime { get; private set; }

		readonly ULogger      _log;
		readonly ISave        _save;
		readonly IEvent       _event;
		readonly GameSettings _settings;

		State _state;

		DateTime _startTime;
		bool     _firstTicked;
		bool     _paused;
		int      _prevHours;

		public TimeController(ILog log, ISave save, IEvent events, GameSettings settings) {
			_log       = log.CreateLogger(this);
			_save      = save;
			_event     = events;
			_settings  = settings;
			Load();
		}

		void Load() {
			_startTime = DateTime.MinValue;
			_state = _save.GetNode<State>(false);
			if ( _state == null ) {
				_state = new State();
				_state.Time = _startTime;
			}
		}

		void Save() {
			_save.SaveNode(_state);
		}

		public void Tick() {
			DeltaTime = _paused ? 0 : Time.deltaTime * _settings.TimeScale;
			_state.Time = GameTime.AddSeconds(DeltaTime);
			if ( !_firstTicked ) {
				_event.Fire(new Time_Started());
				_firstTicked = true;
			}
			var hoursDelta = (int)(GameTime - _startTime).TotalHours;
			while ( hoursDelta > _prevHours ) {
				Save();
				_log.MessageFormat("New game hour: {0}", GameTime);
				_event.Fire(new Time_NewGameHour(GameTime));
				_prevHours++;
			}
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
