using System.Collections.Generic;
using UDBase.Controllers.EventSystem;
using UDBase.Utils;
using UnityEngine;
using Zenject;

public class RequestSpawner : MonoBehaviour {
	public RequestView RequestPrefab;
	public List<Transform> Points;
	public Transform Target;

	IEvent _events;

	Dictionary<Request, RequestView> _views = new Dictionary<Request, RequestView>();

	[Inject]
	public void Init(IEvent events) {
		_events = events;
	}

	void OnEnable() {
		_events.Subscribe<RequestNewStatus>(this, OnRequestNewStatus);
	}

	void OnDisable() {
		_events.Unsubscribe<RequestNewStatus>(OnRequestNewStatus);
	}

	Vector3 GetRandSpawnPointPos() {
		return RandomUtils.GetItem(Points).position;
	}

	void OnRequestNewStatus(RequestNewStatus e) {
		var req = e.Request;
		switch ( e.Status ) {
			case RequestStatus.Incoming: {
					var pos = GetRandSpawnPointPos();
					if ( _views.ContainsKey(req) ) {
						return;
					}
					var view = ObjectPool.Spawn(RequestPrefab, pos);
					view.StartPos = pos;
					view.EndPos = Target.position;
					_views.Add(req, view);
				}
				break;

			case RequestStatus.Outgoing: {
					RequestView view;
					if ( _views.TryGetValue(req, out view) ) {
						view.StartPos = view.transform.position;
						view.EndPos = GetRandSpawnPointPos();
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
			var req = pair.Key;
			var view = pair.Value;
			view.transform.position = Vector3.Lerp(view.StartPos, view.EndPos, req.NormalizedProgress);
		}
	}
}
