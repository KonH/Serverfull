using System;
using UDBase.Controllers.EventSystem;
using Serverfull.Events;
using Serverfull.Models;
using Zenject;

namespace Serverfull.Controllers {
	public class TransportController : IInitializable, IDisposable {
		readonly IEvent            _events;
		readonly ServerController  _server;
		readonly RequestController _request;

		public TransportController(IEvent events, ServerController server, RequestController request) {
			_events  = events;
			_server  = server;
			_request = request;
		}

		public void Initialize() {
			_events.Subscribe<Request_CompleteProgress>(this, OnCompleteProgress);
		}

		public void Dispose() {
			_events.Unsubscribe<Request_CompleteProgress>(OnCompleteProgress);
		}

		void OnCompleteProgress(Request_CompleteProgress e) {
			var req = _request.Get(e.Id);
			switch ( e.CompletedStatus ) {
				case RequestStatus.Awaiting: {
						var target = _server.GetServerForRequest(req);
						if ( (target != null) && _server.TryLockResource(target, Server.Network, req.WantedNetwork) ) {
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
}
