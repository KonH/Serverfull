using System;
using UDBase.Controllers.EventSystem;
using Serverfull.Events;
using Serverfull.Models;
using Serverfull.Game;
using Zenject;

namespace Serverfull.Controllers {
	public class ProcessingController : IInitializable, IDisposable {
		readonly IEvent            _events;
		readonly GameRules         _rules;
		readonly ServerController  _server;
		readonly RequestController _request;
		readonly UserController    _user;

		public ProcessingController(IEvent events, GameRules rules, ServerController server, RequestController request, UserController user) {
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
			var target = req.Target;
			switch ( e.CompletedStatus ) {
				case RequestStatus.Incoming: {
						if ( _server.TryLockResource(target, Server.CPU, req.WantedCPU) ) {
							if ( _server.TryLockResource(target, Server.RAM, req.WantedRAM) ) {
								req.ToProcessing(_rules.GetProcessTime(req.Target));
								return;
							}
						}
						_user.OnRequestFailed(req.Owner);
						req.ToOutgoing(_rules.GetNetworkTime(req.Target));
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
}
