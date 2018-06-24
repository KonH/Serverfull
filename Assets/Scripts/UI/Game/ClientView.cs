using UnityEngine;
using UnityEngine.UI;
using Serverfull.Models;

namespace Serverfull.UI.Game {
	public class ClientView : MonoBehaviour {
		public Text NameText;

		public ClientId     Id    { get; private set; }
		public ClientsPanel Owner { get; private set; }
		
		public void Init(Client client, ClientsPanel owner) {
			Id            = client.Id;
			Owner         = owner;
			NameText.text = client.Id.Name;

			gameObject.name = string.Format("{0}_View", Id.Name);
		}
	}
}
