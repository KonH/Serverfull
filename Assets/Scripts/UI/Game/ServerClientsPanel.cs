using Serverfull.Models;
using Serverfull.Controllers;
using Zenject;

namespace Serverfull.UI.Game {
	public class ServerClientsPanel : ClientsPanel {
		public ServerId ServerId;

		ClientController _client;
		ServerController _server;

		[Inject]
		public void Init(ClientController client, ServerController server) {
			_client = client;
			_server = server;
		}

		protected override void Update() {
			base.Update();
			var server = _server.Get(ServerId);
			if ( server != null ) {
				if ( NeedToUpdate(server.Clients) ) {
					var fullClients = _client.Get(server.Clients);
					Hide();
					Show(fullClients);
				}
			} else {
				Hide();
			}
		}

		public override bool AddClient(ClientView view) {
			var client = _client.Get(view.Id);
			if ( client != null ) {
				var clientServer = _server.GetClientServer(view.Id);
				if ( clientServer == null ) {
					_server.AddClientToServer(view.Id, ServerId);
					return true;
				}
			}
			return false;
		}
	}
}