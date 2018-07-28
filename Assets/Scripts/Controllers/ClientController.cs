using System.Collections.Generic;
using UDBase.Utils;
using UDBase.Controllers.LogSystem;
using UDBase.Controllers.EventSystem;
using Serverfull.Models;
using Serverfull.Events;

namespace Serverfull.Controllers {
	public class ClientController {
		static List<ClientId> _tempIds     = new List<ClientId>();
		static List<Client>   _tempClients = new List<Client>();

		public IEnumerable<Client> All => _clients.Values;

		readonly ULogger          _log;
		readonly ServerController _server;
		readonly IEvent           _event;

		Dictionary<ClientId, Client> _clients = new Dictionary<ClientId, Client>();

		public ClientController(ServerController server, IEvent events) {
			_server = server;
			_event  = events;
		}

		public void AddClient(Client client) {
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

		public void UpdateMood(ClientId id, float change) {
			var client = Get(id);
			client?.UpdateMood(change);
		}

		public void RemoveClient(ClientId id) {
			var server = _server.GetClientServer(id);
			if ( server != null ) {
				_server.RemoveClientFromServer(id, server.Id);
			}
			_event.Fire(new Client_Lost(id));
			_clients.Remove(id);
		}
	}
}
