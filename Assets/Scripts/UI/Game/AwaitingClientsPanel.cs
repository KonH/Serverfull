using UnityEngine;
using Serverfull.Controllers;
using Zenject;

namespace Serverfull.UI.Game {
	[RequireComponent(typeof(ClientsPanel))]
	public class AwaitingClientsPanel : MonoBehaviour {
		ClientController _client;
		ClientsPanel     _clientsPanel;

		[Inject]
		public void Init(ClientController client) {
			_client = client;
		}

		void Start() {
			_clientsPanel = GetComponent<ClientsPanel>();
		}

		void Update() {
			var clientIds = _client.GetAwaitingClients();
			if ( _clientsPanel.NeedToUpdate(clientIds) ) {
				_clientsPanel.Hide();
				var fullClients = _client.Get(clientIds);
				_clientsPanel.Show(fullClients);
			}
		}
	}
}