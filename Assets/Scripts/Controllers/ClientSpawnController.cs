using System;
using UDBase.Controllers.EventSystem;
using Serverfull.Common;
using Serverfull.Events;
using Zenject;

namespace Serverfull.Controllers {
	public class ClientSpawnController : IInitializable, IDisposable {
		readonly IEvent           _event;
		readonly GameSettings     _settings;
		readonly ClientController _client;
		readonly ClientGenerator  _generator;

		int _lastHours;

		public ClientSpawnController(IEvent events, GameSettings settings, ClientController client, ClientGenerator generator) {
			_event     = events;
			_settings  = settings;
			_client    = client;
			_generator = generator;
		}

		public void Initialize() {
			_event.Subscribe<Time_NewGameHour>(this, OnNewHour);
			SpawnClients(_settings.FirstClientSpawn);
		}

		public void Dispose() {
			_event.Unsubscribe<Time_NewGameHour>(OnNewHour);
		}

		void OnNewHour(Time_NewGameHour e) {
			var newHours = (int)(e.GameTime - DateTime.MinValue).TotalHours;
			if ( newHours > _lastHours + _settings.ClientSpawnInterval ) {
				_lastHours = newHours;
				SpawnClients(_settings.ClientsPerSpawn);
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
