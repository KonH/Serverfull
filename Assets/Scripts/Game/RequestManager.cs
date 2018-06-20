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
		public Transform       Target;

		IEvent            _events;
		RequestController _request;

		Dictionary<RequestId, RequestView> _views = new Dictionary<RequestId, RequestView>();

		[Inject]
		public void Init(IEvent events, RequestController request) {
			_events  = events;
			_request = request;
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
			var req = e.Id;
			switch ( e.NewStatus ) {
				case RequestStatus.Incoming: {
						var pos = GetRandSpawnPointPos();
						if ( _views.ContainsKey(req) ) {
							return;
						}
						var view = ObjectPool.Spawn(RequestPrefab, pos);
						view.StartPos = pos;
						view.EndPos   = Target.position;
						_views.Add(req, view);
					}
					break;

				case RequestStatus.Outgoing: {
						RequestView view;
						if ( _views.TryGetValue(req, out view) ) {
							view.StartPos = view.transform.position;
							view.EndPos   = GetRandSpawnPointPos();
						}
					}
					break;

				case RequestStatus.Finished: {
						RequestView view;
						if ( _views.TryGetValue(req, out view) ) {
							ObjectPool.Recycle(view);
							_views.Remove(req);
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
				view.MoodRenderer.material.color = Color.Lerp(view.BadMoodColor, view.GoodMoodColor, req.Owner.Mood);
			}
		}
	}
}
