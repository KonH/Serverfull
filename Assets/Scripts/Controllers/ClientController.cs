using System.Collections.Generic;
using UDBase.Controllers.LogSystem;
using Serverfull.Models;

namespace Serverfull.Controllers {
	public class ClientController {
		readonly ULogger          _log;
		readonly ServerController _server;

		Dictionary<ClientId, Client> _clients = new Dictionary<ClientId, Client>();

		public ClientController(ServerController server) {
			_server = server;
			AddClient(new Client(new ClientId("Client1"), new Money(10), 1));
		}

		void AddClient(Client client) {
			_clients.Add(client.Id, client);
			_server.AddClientToServer(client.Id, new ServerId(1));
		}

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
