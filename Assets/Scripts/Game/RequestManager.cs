using System.Collections.Generic;
using UnityEngine;
using UDBase.Utils;
using UDBase.Controllers.EventSystem;
using Serverfull.Views;
using Serverfull.Events;
using Serverfull.Models;
using Serverfull.Controllers;
using Zenject;

namespace Serverfull.Game {
	public class RequestManager : MonoBehaviour {
		public RequestView     RequestPrefab;
		public List<Transform> Points;

		IEvent            _events;
		RequestController _request;
		ServerManager     _server;

		Dictionary<RequestId, RequestView> _views = new Dictionary<RequestId, RequestView>();

		[Inject]
		public void Init(IEvent events, RequestController request, ServerManager server) {
			_events  = events;
			_request = request;
			_server  = server;
		}

		void OnEnable() {
			_events.Subscribe<Request_NewStatus>(this, OnNewStatus);
		}

		void OnDisable() {
			_events.Unsubscribe<Request_NewStatus>(OnNewStatus);
		}

		Vector3 GetRandSpawnPointPos() {
			return RandomUtils.GetItem(Points).position;
		}

		void OnNewStatus(Request_NewStatus e) {
			var id = e.Id;
			switch ( e.NewStatus ) {
				case RequestStatus.Incoming: {
						var pos = GetRandSpawnPointPos();
						if ( _views.ContainsKey(id) ) {
							return;
						}
						var req = _request.Get(id);
						if ( req != null ) {
							var targetServerView = _server.GetView(req.Target.Id);
							if ( targetServerView != null ) {
								var view = ObjectPool.Spawn(RequestPrefab, pos);
								view.StartPos = pos;
								view.EndPos = targetServerView.transform.position;
								_views.Add(id, view);
							}
						}
					}
					break;

				case RequestStatus.Outgoing: {
						RequestView view;
						if ( _views.TryGetValue(id, out view) ) {
							view.StartPos = view.transform.position;
							view.EndPos   = GetRandSpawnPointPos();
						}
					}
					break;

				case RequestStatus.Finished: {
						RequestView view;
						if ( _views.TryGetValue(id, out view) ) {
							ObjectPool.Recycle(view);
							_views.Remove(id);
						}
					}
					break;
			}
		}

		void Update() {
			foreach ( var pair in _views ) {
				var req      = _request.Get(pair.Key);
				var view     = pair.Value;
				var progress = req.Status != RequestStatus.Processing ? req.NormalizedProgress : 1.0f;
				view.transform.position = Vector3.Lerp(view.StartPos, view.EndPos, progress);
				foreach ( var md in view.MoodRenderers ) {
					md.material.color = Color.Lerp(view.BadMoodColor, view.GoodMoodColor, req.Owner.Mood);
				}
			}
		}
	}
}
