using Serverfull.Controllers;
using Zenject;

namespace Serverfull.UI.Game {
	public class AwaitingClientsPanel : ClientsPanel {
		ClientController _client;
		ServerController _server;

		[Inject]
		public void Init(ClientController client, ServerController server) {
			_client = client;
			_server = server;
		}

		protected override void Update() {
			base.Update();
			var clientIds = _client.GetAwaitingClients();
			if ( NeedToUpdate(clientIds) ) {
				Hide();
				var fullClients = _client.Get(clientIds);
				Show(fullClients);
			}
		}

		public override bool AddClient(ClientView view) {
			var client = _client.Get(view.Id);
			if ( client != null ) {
				var serverPanel = view.Owner as ServerClientsPanel;
				if ( serverPanel != null ) {
					var serverId = serverPanel.ServerId;
					_server.RemoveClientFromServer(view.Id, serverId);
					return true;
				}
			}
			return false;
		}
	}
}