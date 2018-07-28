using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.EventSystem;
using Serverfull.Views;
using Serverfull.Models;
using Serverfull.Events;
using Zenject;

namespace Serverfull.Game {
	public class ServerManager : MonoBehaviour {
		public ServerView ServerViewPrefab;

		IEvent _event;

		Dictionary<ServerId, ServerView> _views    = new Dictionary<ServerId, ServerView>();
		ServerView                       _selected = null;

		[Inject]
		public void Init(IEvent events) {
			_event = events;
		}

		void OnEnable() {
			_event?.Subscribe<Server_New>(this, OnNewServer);
		}

		void OnDisable() {
			_event?.Unsubscribe<Server_New>(OnNewServer);
		}

		void OnNewServer(Server_New e) {
			var view = ObjectPool.Spawn(ServerViewPrefab, new Vector3(e.PosX, 0, e.PosY));
			view.Init(e.Id);
			view.SetSelected(false);
			_views.Add(e.Id, view);
		}

		public ServerView GetView(ServerId id) {
			ServerView view;
			_views.TryGetValue(id, out view);
			return view;
		}

		public void UpdateSelectedServer(ServerId id) {
			if ( _selected ) {
				_selected.SetSelected(false);
			}
			var view = GetView(id);
			_selected = view;
			view.SetSelected(true);
		}
	}
}
