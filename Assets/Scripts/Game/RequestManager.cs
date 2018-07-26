using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.EventSystem;
using Serverfull.Views;
using Serverfull.Events;
using Serverfull.Models;
using Serverfull.Controllers;
using Zenject;

namespace Serverfull.Game {
	public class RequestManager : MonoBehaviour {
		public RequestView RequestPrefab;
		public float       SpawnDistance;

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

		Vector3 GetRandSpawnPointPos(Vector3 centerPos) {
			var angle = Random.value * 360;
			var x = SpawnDistance * Mathf.Cos(angle);
			var z = SpawnDistance * Mathf.Sin(angle);
			return new Vector3(x, 0, z);
		}

		Vector3 GetServerViewCenter(RequestId id) {
			var req = _request.Get(id);
			if ( (req != null) && (req.Target != null)  ) {
				var targetServerView = _server.GetView(req.Target.Id);
				if ( targetServerView != null ) {
					return targetServerView.Center.position;
				}
			}
			return Vector3.zero;
		}

		RequestView SpawnView(RequestId id) {
			var serverCenter = GetServerViewCenter(id);
			var spawnPos = GetRandSpawnPointPos(serverCenter);
			var view = ObjectPool.Spawn(RequestPrefab, spawnPos);
			view.StartPos = spawnPos;
			view.EndPos = serverCenter;
			view.Trail.Clear();
			return view;
		}

		RequestView GetOrSpawnView(RequestId id) {
			RequestView view;
			if ( !_views.TryGetValue(id, out view) ) {
				view = SpawnView(id);
				_views.Add(id, view);
			}
			return view;
		}


		void OnNewStatus(Request_NewStatus e) {
			var id = e.Id;
			RequestView view = GetOrSpawnView(id);
			switch ( e.NewStatus ) {
				case RequestStatus.Outgoing: {
						view.StartPos = view.transform.position;
						view.EndPos   = GetRandSpawnPointPos(view.StartPos);
					}
					break;

				case RequestStatus.Finished: {
						ObjectPool.Recycle(view);
						_views.Remove(id);
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
