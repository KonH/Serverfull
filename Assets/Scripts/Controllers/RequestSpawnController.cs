using Serverfull.Models;
using Zenject;

namespace Serverfull.Controllers {
	public class RequestSpawnController : ITickable {
		readonly TimeController    _time;
		readonly ClientController  _client;
		readonly UserController    _user;
		readonly RequestController _request;

		float _spawnTimer = 0.0f;

		public RequestSpawnController(TimeController time, ClientController client, UserController user, RequestController request) {
			_time       = time;
			_client     = client;
			_user       = user;
			_request    = request;
			_spawnTimer = 1;
		}

		public void Tick() {
			_spawnTimer += _time.DeltaTime;
			var interval = 1;
			while ( _spawnTimer > interval ) {
				var clientId = new ClientId("Client1");
				var client = _client.Get(clientId);
				InitiateRequest(client);
				_spawnTimer -= interval;
			}
		}

		void InitiateRequest(Client client) {
			var owner = _user.CreateUser(client.Id);
			var req = new Request(RequestId.Create(), owner, client.WantedNetwork, client.WantedCPU, client.WantedRAM);
			_request.Add(req);
		}
	}
}
