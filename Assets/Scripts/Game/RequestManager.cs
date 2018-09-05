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

		Vector3 GetRandSpawnPointPosAround(Vector3 centerPos) {
			var angle = Random.value * 360;
			var x = centerPos.x + SpawnDistance * Mathf.Cos(angle);
			var z = centerPos.z + SpawnDistance * Mathf.Sin(angle);
			return new Vector3(x, 0, z);
		}

		Vector3 GetServerViewCenter(ServerId id) {
			var serverView = _server.GetView(id);
			if ( serverView != null ) {
				return serverView.Center.position;
			}
			return Vector3.zero;
		}
		
		RequestView SpawnView(RequestId id) {
			var req = _request.Get(id);
			var originServer = req.Origin;
			var targetServer = req.Target;
			if ( targetServer == null ) {
				return null;
			}
			
			var targetServerCenter = GetServerViewCenter(targetServer.Id);
			var startPos = (originServer != null) ? GetServerViewCenter(originServer.Id) : GetRandSpawnPointPosAround(targetServerCenter);
			
			var view = ObjectPool.Spawn(RequestPrefab, startPos);
			view.StartPos = startPos;
			view.EndPos = targetServerCenter;
			view.Trail.Clear();
			return view;
		}

		RequestView GetOrSpawnView(RequestId id) {
			RequestView view;
			if ( !_views.TryGetValue(id, out view) ) {
				view = SpawnView(id);
				if ( view != null ) {
					_views.Add(id, view);
				}
			}
			return view;
		}


		void OnNewStatus(Request_NewStatus e) {
			var id = e.Id;
			var view = GetOrSpawnView(id);
			if ( view == null ) {
				return;
			}
			switch ( e.NewStatus ) {
				case RequestStatus.Outgoing: {
						view.EndPos   = view.StartPos;
						view.StartPos = view.transform.position;
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
					md.material.color = Color.Lerp(view.BadMoodColor, view.GoodMoodColor, req.Owner.NormalizedMood);
				}
			}
		}
	}
}
