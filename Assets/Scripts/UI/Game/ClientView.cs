using UnityEngine;
using UnityEngine.UI;
using Serverfull.Models;

namespace Serverfull.UI.Game {
	public class ClientView : MonoBehaviour {
		public Text NameText;

		public ClientId Id { get; private set; }
		
		public void Init(Client client) {
			Id            = client.Id;
			NameText.text = client.Id.Name;
		}
	}
}
