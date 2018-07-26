﻿using System;
using UDBase.Controllers.EventSystem;
using Serverfull.Game;
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

		public TransportController(IEvent events, GameRules rules, ServerController server, RequestController request, UserController user) {
			_events  = events;
			_rules   = rules;
			_server  = server;
			_request = request;
			_user    = user;
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
						if ( target != null ) {
							if ( _server.TryLockResource(target, target.Network, req.WantedNetwork) ) {
								req.ToIncoming(target, _rules.GetNetworkTime(target));
							} else {
								_user.OnRequestFailed(req.Owner);
								req.ToFinished();
							}
						}
					}
					break;

				case RequestStatus.Incoming: {
						_server.ReleaseResource(req.Target, req.Target.Network, req.WantedNetwork);
					}
					break;

				case RequestStatus.Processing: {
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
