using System;
using UDBase.Controllers.EventSystem;
using Zenject;

public class TransportController : IInitializable, IDisposable {
	readonly IEvent _events;
	readonly ServerController _server;

	public TransportController(IEvent events, ServerController server) {
		_events = events;
		_server = server;
	}

	public void Initialize() {
		_events.Subscribe<RequestReady>(this, OnRequestReady);
	}

	public void Dispose() {
		_events.Unsubscribe<RequestReady>(OnRequestReady);
	}

	void OnRequestReady(RequestReady e) {
		var req = e.Request;
		switch ( e.Status ) {
			case RequestStatus.Awaiting: {
					var target = _server.GetServerForRequest(req);
					if ( _server.TryLockResource(target, Server.Network, req.WantedNetwork) ) {
						req.ToIncoming(target, target.NetworkTime);
					}
				}
				break;

			case RequestStatus.Incoming: {
					_server.ReleaseResource(req.Target, Server.Network, req.WantedNetwork);
				}
				break;

			case RequestStatus.Processing: {
					req.ToOutgoing(req.Target.NetworkTime);
				}
				break;

			case RequestStatus.Outgoing: {
					req.ToFinished();
				}
				break;
		}
	}
}
