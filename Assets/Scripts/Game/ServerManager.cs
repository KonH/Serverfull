using System.Collections.Generic;
using UnityEngine;
using Serverfull.Views;
using Serverfull.Models;

namespace Serverfull.Game {
	public class ServerManager : MonoBehaviour {
		Dictionary<ServerId, ServerView> _views = new Dictionary<ServerId, ServerView>();

		void Start() {
			var startupViews = GameObject.FindObjectsOfType<ServerView>();
			foreach ( var view in startupViews ) {
				_views.Add(view.Id, view);
			}
		}

		public ServerView GetView(ServerId id) {
			ServerView view;
			_views.TryGetValue(id, out view);
			return view;
		}
	}
}
