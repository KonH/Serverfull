using System;
using System.ComponentModel;
using Serverfull.Common;
using Serverfull.Models;
using Zenject;

namespace Serverfull.Controllers {
	public class EngineerSpawnController : IInitializable, IDisposable {
		readonly GameSettings       _settings;
		readonly TimeController     _time;
		readonly EngineerController _engineer;
		readonly EngineerGenerator  _generator;

		int _lastHours;

		public EngineerSpawnController(GameSettings settings, TimeController time, EngineerController engineer, EngineerGenerator generator) {
			_settings  = settings;
			_time      = time;
			_engineer  = engineer;
			_generator = generator;
		}

		public void Initialize() {
			_time.State.PropertyChanged += OnTimeChanged;
			SpawnEngineers(_settings.FirstEngineerSpawn);
		}

		public void Dispose() {
			_time.State.PropertyChanged -= OnTimeChanged;
		}

		void OnTimeChanged(object sender, PropertyChangedEventArgs e) {
			if ( e.PropertyName == nameof(TimeModel.Hour) ) {
				var newHours = _time.State.Hour;
				if ( newHours > _lastHours + _settings.EngineerSpawnInterval ) {
					_lastHours = newHours;
					SpawnEngineers(_settings.ClientsPerSpawn);
				}
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
