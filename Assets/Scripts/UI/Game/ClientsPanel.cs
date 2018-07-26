using UnityEngine;
using Serverfull.Models;
using System.Collections.Generic;
using Serverfull.Controllers;
using Zenject;

namespace Serverfull.UI.Game {
	public abstract class ClientsPanel : MonoBehaviour {
		public static HashSet<ClientsPanel> Instances { get; private set; } = new HashSet<ClientsPanel>();

		public ClientView    Prefab;
		public RectTransform ContentRoot;

		protected List<ClientView> _views = new List<ClientView>();

		ClientController _client; 

		void OnEnable() {
			Instances.Add(this);
		}

		void OnDisable() {
			Instances.Remove(this);
		}

		[Inject]
		public void Init(ClientController client) {
			_client = client;
		}

		public void Show(List<Client> clients) {
			foreach ( var client in clients ) {
				var view = ObjectPool.Spawn(Prefab, ContentRoot);
				view.Init(client, this);
				_views.Add(view);
			}
		}

		protected virtual void Update() {
			foreach ( var view in _views ) {
				var client = _client.Get(view.Id);
				if ( client != null ) {
					view.UpdateMood(client.Mood);
				}
			}
		}

		public void Hide() {
			foreach ( var view in _views ) {
				ObjectPool.Recycle(view);
			}
			_views.Clear();
		}
		
		public bool NeedToUpdate(List<ClientId> clients) {
			if ( clients.Count != _views.Count ) {
				return true;
			}
			foreach ( var view in _views ) {
				if ( !clients.Contains(view.Id) ) {
					return true;
				}
			}
			return false;
		}

		public void FreeView(ClientView view) {
			_views.Remove(view);
			ObjectPool.Recycle(view);
		}

		public abstract bool AddClient(ClientView view);
	}
}
