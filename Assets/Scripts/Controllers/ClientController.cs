using System.Collections.Generic;
using UDBase.Utils;
using UDBase.Controllers.LogSystem;
using Serverfull.Models;

namespace Serverfull.Controllers {
	public class ClientController {
		public IEnumerable<Client> All => _clients.Values;

		readonly ULogger          _log;
		readonly ServerController _server;

		Dictionary<ClientId, Client> _clients = new Dictionary<ClientId, Client>();

		public ClientController(ServerController server) {
			_server = server;
			AddClient(new Client(new ClientId("Client1"), new Money(1), 1, 1, 1, 3), new ServerId(1));
			AddClient(new Client(new ClientId("Client2"), new Money(10), 10, 5, 1, 1), new ServerId(2));
			AddClient(new Client(new ClientId("Client3"), new Money(100), 25, 1, 5, 2), new ServerId(3));
		}

		void AddClient(Client client, ServerId server) {
			_clients.Add(client.Id, client);
			_server.AddClientToServer(client.Id, server);
		}

		public Client Get(ClientId id) => _clients.GetOrDefault(id);

		public Money GetTotalIncome() {
			var result = Money.Zero;
			foreach ( var client in _clients ) {
				if ( _server.GetClientServer(client.Key) != null ) {
					result += client.Value.Income;
				}
			}
			return result;
		}
	}
}
