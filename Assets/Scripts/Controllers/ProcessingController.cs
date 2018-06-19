using System;
using UDBase.Controllers.EventSystem;
using Zenject;

public class ProcessingController : IInitializable, IDisposable {
	readonly IEvent _events;
	readonly ServerController _server;
	readonly UserController _user;

	public ProcessingController(IEvent events, ServerController server, UserController user) {
		_events = events;
		_server = server;
		_user = user;
	}

	public void Initialize() {
		_events.Subscribe<RequestReady>(this, OnRequestReady);
	}

	public void Dispose() {
		_events.Unsubscribe<RequestReady>(OnRequestReady);
	}

	void OnRequestReady(RequestReady e) {
		var req = e.Request;
		var target = req.Target;
		switch ( e.Status ) {
			case RequestStatus.Incoming: {
					if ( _server.TryLockResource(target, Server.CPU, req.WantedCPU) ) {
						if ( _server.TryLockResource(target, Server.RAM, req.WantedRAM) ) {
							req.ToProcessing(req.Target.NetworkTime);
							return;
						}
					}
					_user.OnRequestFailed(req.Owner);
					req.ToOutgoing(req.Target.NetworkTime);
				}
				break;

			case RequestStatus.Processing: {
					_server.ReleaseResource(target, Server.CPU, req.WantedCPU);
					_server.ReleaseResource(target, Server.RAM, req.WantedRAM);
				}
				break;
		}
	}
}
