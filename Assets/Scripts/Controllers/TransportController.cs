using System;
using UDBase.Controllers.EventSystem;
using Serverfull.Common;
using Serverfull.Events;
using Serverfull.Models;
using Zenject;

namespace Serverfull.Controllers {
	public class TransportController : IInitializable, IDisposable {
		readonly IEvent            _events;
		readonly GameRules         _rules;
		readonly ServerController  _server;
		readonly RequestController _request;
		readonly UserController    _user;
		readonly BreakController   _break;

		public TransportController(IEvent events, GameRules rules, ServerController server, RequestController request, UserController user, BreakController breaking) {
			_events  = events;
			_rules   = rules;
			_server  = server;
			_request = request;
			_user    = user;
			_break   = breaking;
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
						var origin = req.IsMainRequest ? null : _request.GetMainRequestForUser(req.Owner).Target; 
						var target = _server.GetServerForRequest(req);
						if ( target != null ) {
							req.ToIncoming(origin, target, _rules.GetNetworkTime(target));
							return;
						}
						_user.OnRequestFailed(req.Owner);
						req.ToFinished();
					}
					break;

				case RequestStatus.Incoming: {
						if ( _break.IsServerBreaked(req.Target.Id) ) {
							return;
						}
						_server.ReleaseResource(req.Target, req.Target.Network, req.WantedNetwork);
					}
					break;

				case RequestStatus.Processing: {
						if ( req.IsMainRequest ) {
							// Wait for any other related requests
							var reqs = _request.GetRequestsForUser(req.Owner);
							foreach ( var r in reqs ) {
								if ( r != req ) {
									return;
								}
							}
						}
						req.ToOutgoing(_rules.GetNetworkTime(req.Target));
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
