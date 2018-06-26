using System.Collections.Generic;
using UDBase.Utils;
using UDBase.Controllers.LogSystem;
using Serverfull.Models;

namespace Serverfull.Controllers {
	public class ClientController {
		static List<ClientId> _tempIds     = new List<ClientId>();
		static List<Client>   _tempClients = new List<Client>();

		public IEnumerable<Client> All => _clients.Values;

		readonly ULogger          _log;
		readonly ServerController _server;

		Dictionary<ClientId, Client> _clients = new Dictionary<ClientId, Client>();

		public ClientController(ServerController server) {
			_server = server;
			AddClient(new Client(new ClientId("Client1"), new Money(1), 1, 1, 1, 1));
			AddClient(new Client(new ClientId("Client2"), new Money(10), 1, 1, 1, 1));
			AddClient(new Client(new ClientId("Client3"), new Money(100), 1, 1, 1, 1));
			AddClient(new Client(new ClientId("Client4"), new Money(100), 1, 1, 1, 1));
		}

		void AddClient(Client client) {
			_clients.Add(client.Id, client);
		}

		public Client Get(ClientId id) => _clients.GetOrDefault(id);

		public List<Client> Get(List<ClientId> ids) {
			_tempClients.Clear();
			var result = _tempClients;
			foreach ( var id in ids ) {
				var client = Get(id);
				if ( client != null ) {
					result.Add(client);
				}
			}
			return result;
		}

		public bool IsAssignedToServer(ClientId id) {
			return _server.GetClientServer(id) != null;
		}

		public List<ClientId> GetAwaitingClients() {
			_tempIds.Clear();
			var result = _tempIds;
			foreach ( var id in _clients.Keys ) {
				if ( !IsAssignedToServer(id) ) {
					result.Add(id);
				}
			}
			return result;
		}

		public Money GetTotalIncome() {
			var result = Money.Zero;
			foreach ( var client in _clients ) {
				if ( IsAssignedToServer(client.Key)) {
					result += client.Value.Income;
				}
			}
			return result;
		}
	}
}
