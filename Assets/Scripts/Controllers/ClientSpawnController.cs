using System;
using System.ComponentModel;
using Serverfull.Common;
using Serverfull.Models;
using Zenject;

namespace Serverfull.Controllers {
	public class ClientSpawnController : IInitializable, IDisposable {
		readonly GameSettings     _settings;
		readonly TimeController   _time;
		readonly ClientController _client;
		readonly ClientGenerator  _generator;

		int _lastHours;

		public ClientSpawnController(GameSettings settings, TimeController time, ClientController client, ClientGenerator generator) {
			_settings  = settings;
			_time      = time;
			_client    = client;
			_generator = generator;
		}

		public void Initialize() {
			_time.State.PropertyChanged += OnTimeChanged;
			SpawnClients(_settings.FirstClientSpawn);
		}

		public void Dispose() {
			_time.State.PropertyChanged -= OnTimeChanged;
		}

		void OnTimeChanged(object sender, PropertyChangedEventArgs e) {
			if ( e.PropertyName == nameof(TimeModel.Hour) ) {
				var newHours = _time.State.Hour;
				if ( newHours > _lastHours + _settings.ClientSpawnInterval ) {
					_lastHours = newHours;
					SpawnClients(_settings.ClientsPerSpawn);
				}
			}
		}

		void SpawnClients(int count) {
			for ( var i = 0; i < count; i++ ) {
				var client = _generator.CreateClient();
				if ( client != null ) {
					_client.AddClient(client);
				}
			}
		}
	}
}
