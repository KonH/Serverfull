using System.Collections.Generic;
using Serverfull.Common;
using Serverfull.Models;
using UDBase.Utils;
using Zenject;

namespace Serverfull.Controllers {
	public class RequestSpawnController : ITickable {
		const float SecondsInHour = 60 * 60;
		
		readonly TimeController    _time;
		readonly ClientController  _client;
		readonly UserController    _user;
		readonly RequestController _request;

		List<Client>                _clients     = new List<Client>();
		Dictionary<ClientId, float> _spawnTimers = new Dictionary<ClientId, float>();

		public RequestSpawnController(TimeController time, ClientController client, UserController user, RequestController request) {
			_time     = time;
			_client   = client;
			_user     = user;
			_request  = request;
		}

		public void Tick() {
			_clients.Clear();
			_clients.AddRange(_client.All);
			while ( _clients.Count > 0 ) {
				var client = RandomUtils.GetItem(_clients);
				_clients.Remove(client);
				var curTimer = 0.0f;
				var spawnDelta = GetSpawnDelta(client.UserRate);
				if ( !_spawnTimers.TryGetValue(client.Id, out curTimer) ) {
					curTimer = spawnDelta;
					_spawnTimers.Add(client.Id, curTimer);
				} else {
					curTimer += _time.DeltaTime;
				}
				while ( curTimer >= spawnDelta ) {
					curTimer -= spawnDelta;
					InitiateRequest(client);
				}
				_spawnTimers[client.Id] = curTimer;
			}
		}

		float GetSpawnDelta(int userRatePerHour) {
			var secPerUser = SecondsInHour / userRatePerHour;
			return secPerUser;
		}

		void InitiateRequest(Client client) {
			if ( !_client.IsAssignedToServer(client.Id) ) {
				return;
			}
			var owner = _user.CreateUser(client.Id);
			var req = new Request(RequestId.Create(), owner, client.WantedNetwork, client.WantedCPU, client.WantedRAM);
			_request.Add(req);
		}
	}
}
