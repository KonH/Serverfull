using System;
using UDBase.Controllers.EventSystem;
using Serverfull.Common;
using Serverfull.Events;
using Zenject;

namespace Serverfull.Controllers {
	public class EngineerSpawnController : IInitializable, IDisposable {
		readonly IEvent             _event;
		readonly GameSettings       _settings;
		readonly EngineerController _engineer;
		readonly EngineerGenerator  _generator;

		int _lastHours;

		public EngineerSpawnController(IEvent events, GameSettings settings, EngineerController engineer, EngineerGenerator generator) {
			_event     = events;
			_settings  = settings;
			_engineer  = engineer;
			_generator = generator;
		}

		public void Initialize() {
			_event.Subscribe<Time_NewGameHour>(this, OnNewHour);
			SpawnEngineers(_settings.FirstEngineerSpawn);
		}

		public void Dispose() {
			_event.Unsubscribe<Time_NewGameHour>(OnNewHour);
		}

		void OnNewHour(Time_NewGameHour e) {
			var newHours = (int)(e.GameTime - DateTime.MinValue).TotalHours;
			if ( newHours > _lastHours + _settings.EngineerSpawnInterval ) {
				_lastHours = newHours;
				SpawnEngineers(_settings.ClientsPerSpawn);
			}
		}

		void SpawnEngineers(int count) {
			for ( var i = 0; i < count; i++ ) {
				var engineer = _generator.CreateEngineer();
				if ( engineer != null ) {
					_engineer.AddUnit(engineer);
				}
			}
		}
	}
}
