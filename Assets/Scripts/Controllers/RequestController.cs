using System.Collections.Generic;
using UDBase.Utils;
using UDBase.Controllers.EventSystem;
using UDBase.Controllers.LogSystem;
using Serverfull.Common;
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
		List<Request>                  _newRequests      = new List<Request>();

		public RequestController(ILog log, IEvent events, TimeController time, UserController user) {
			_log        = log.CreateLogger(this);
			_events     = events;
			_time       = time;
			_user       = user;
		}

		public Request Get(RequestId id) => _requests.GetOrDefault(id);

		public void Add(Request request) {
			_log.MessageFormat("Add: {0}", request);
			_newRequests.Add(request);
		}

		public void Tick() {
			var deltaTime = _time.DeltaTime;
			foreach ( var req in _requests.Values ) {
				if ( req.IsFinished ) {
					_finishedRequests.Add(req.Id);
				} else {
					if ( req.UpdateProgress(deltaTime) ) {
						UpdateRequest(req, deltaTime);
					}
				}
			}
			foreach ( var req in _finishedRequests ) {
				_requests.Remove(req);
			}
			_finishedRequests.Clear();
			foreach ( var req in _newRequests ) {
				_requests.Add(req.Id, req);
			}
			_newRequests.Clear();
		}

		void UpdateRequest(Request req, float deltaTime) {
			var status = req.Status;
			_events.Fire(new Request_CompleteProgress(req));
			if ( req.Status != status ) {
				_events.Fire(new Request_NewStatus(req));
			}
		}

		public void AddRelatedRequests(Request req) {
			var addServers = _user.GetAdditionalServers(req.Owner);
			if ( addServers == null ) {
				return;
			}
			foreach ( var serverType in addServers ) {
				var newReq = new Request(RequestId.Create(), serverType, req.Owner, req.WantedNetwork, req.WantedCPU, req.WantedRAM);
				Add(newReq);
			}
		}

		public List<Request> GetRequestsForUser(User user) {
			var result = new List<Request>();
			foreach ( var req in _requests.Values ) {
				if ( req.Owner == user ) {
					result.Add(req);
				}
			}
			return result;
		}
		
		public Request GetMainRequestForUser(User user) {
			foreach ( var req in _requests.Values ) {
				if ( req.IsMainRequest && (req.Owner == user) ) {
					return req;
				}
			}
			return null;
		}
	}
}
