using System;
using UDBase.Controllers.EventSystem;
using Serverfull.Events;
using Serverfull.Models;
using Serverfull.Common;
using Zenject;

namespace Serverfull.Controllers {
	public class ProcessingController : IInitializable, IDisposable {
		readonly IEvent            _events;
		readonly GameRules         _rules;
		readonly ServerController  _server;
		readonly RequestController _request;
		readonly UserController    _user;
		readonly BreakController   _break;

		public ProcessingController(IEvent events, GameRules rules, ServerController server, RequestController request, UserController user, BreakController breaking) {
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
			var target = req.Target;
			switch ( e.CompletedStatus ) {
				case RequestStatus.Incoming: {
						if ( _server.TryLockResource(target, target.CPU, req.WantedCPU) ) {
							if ( _server.TryLockResource(target, target.RAM, req.WantedRAM) ) {
								req.ToProcessing(_rules.GetProcessTime(req.Target));
								return;
							}
						}
						_user.OnRequestFailed(req.Owner);
						req.ToOutgoing(_rules.GetNetworkTime(req.Target));
					}
					break;

				case RequestStatus.Processing: {
						if ( _break.IsServerBreaked(target.Id) ) {
							return;
						}
						_server.ReleaseResource(target, target.CPU, req.WantedCPU);
						_server.ReleaseResource(target, target.RAM, req.WantedRAM);
					}
					break;
			}
		}
	}
}
