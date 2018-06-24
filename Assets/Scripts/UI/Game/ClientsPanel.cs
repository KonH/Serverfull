using UnityEngine;
using Serverfull.Models;
using System.Collections.Generic;

namespace Serverfull.UI.Game {
	public class ClientsPanel : MonoBehaviour {
		public ClientView Prefab;
		public Transform ContentRoot;

		List<ClientView> _views = new List<ClientView>();
		
		public void Show(List<Client> clients) {
			foreach ( var client in clients ) {
				var view = ObjectPool.Spawn(Prefab, ContentRoot);
				view.Init(client);
				_views.Add(view);
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
	}
}
