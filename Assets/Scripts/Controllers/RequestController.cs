using System.Collections.Generic;
using UDBase.Utils;
using UDBase.Controllers.EventSystem;
using UDBase.Controllers.LogSystem;
using Serverfull.Game;
using Serverfull.Models;
using Serverfull.Events;
using Zenject;

namespace Serverfull.Controllers {
	public class RequestController : ILogContext, ITickable {
		readonly ULogger        _log;
		readonly IEvent         _events;
		readonly GameSettings   _settings;
		readonly TimeController _time;
		readonly UserController _user;

		Dictionary<RequestId, Request> _requests         = new Dictionary<RequestId, Request>();
		List<RequestId>                _finishedRequests = new List<RequestId>();

		public RequestController(ILog log, IEvent events, TimeController time, UserController user) {
			_log        = log.CreateLogger(this);
			_events     = events;
			_time       = time;
			_user       = user;
		}

		public Request Get(RequestId id) => _requests.GetOrDefault(id);

		public void Add(Request request) {
			_log.MessageFormat("Add: {0}", request);
			_requests.Add(request.Id, request);
		}

		public void Tick() {
			var deltaTime = _time.DeltaTime;
			foreach ( var req in _requests.Values ) {
				if ( req.IsFinished ) {
					_finishedRequests.Add(req.Id);
				} else if ( req.UpdateProgress(deltaTime) ) {
					UpdateRequest(req, deltaTime);
				}
			}
			foreach ( var req in _finishedRequests ) {
				_requests.Remove(req);
			}
		}

		void UpdateRequest(Request req, float deltaTime) {
			var status = req.Status;
			_events.Fire(new Request_CompleteProgress(req));
			if ( req.Status != status ) {
				_events.Fire(new Request_NewStatus(req));
			}
			_user.UpdateMood(req.Owner, deltaTime);
		}
	}
}
