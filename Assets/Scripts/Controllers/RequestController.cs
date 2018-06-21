using System.Collections.Generic;
using UDBase.Utils;
using UDBase.Controllers.EventSystem;
using UDBase.Controllers.LogSystem;
using Serverfull.Game;
using Serverfull.Models;
using Serverfull.Events;
using Zenject;

namespace Serverfull.Controllers {
	public class RequestController : ITickable, ILogContext {
		readonly ULogger        _log;
		readonly IEvent         _events;
		readonly GameSettings   _settings;
		readonly TimeController _time;
		readonly UserController _user;

		float                          _spawnTimer = 0.0f;
		Dictionary<RequestId, Request> _requests   = new Dictionary<RequestId, Request>();

		public RequestController(ILog log, IEvent events, GameSettings settings, TimeController time, UserController user) {
			_log        = log.CreateLogger(this);
			_events     = events;
			_settings   = settings;
			_time       = time;
			_user       = user;
			_spawnTimer = _settings.RequestSpawnInterval;
		}

		public void Tick() {
			_spawnTimer += _time.DeltaTime;
			var interval = _settings.RequestSpawnInterval;
			while ( _spawnTimer > interval ) {
				InitiateRequest();
				_spawnTimer -= interval;
			}
			UpdateRequests();
		}

		public Request Get(RequestId id) => _requests.GetOrDefault(id);

		void InitiateRequest() {
			var owner = _user.CreateUser();
			var req = new Request(RequestId.Create(), owner, _settings.WantedNetwork, _settings.WantedCPU, _settings.WantedRAM);
			_log.MessageFormat("InitiateRequest: {0}", req);
			_requests.Add(req.Id, req);
		}

		void UpdateRequests() {
			var deltaTime = _time.DeltaTime;
			foreach ( var req in _requests.Values ) {
				if ( !req.IsFinished && req.UpdateProgress(deltaTime) ) {
					UpdateRequest(req, deltaTime);
				}
			}
		}

		void UpdateRequest(Request req, float deltaTime) {
			var status = req.Status;
			_events.Fire(new Request_CompleteProgress(req));
			if ( req.Status != status ) {
				_events.Fire(new Request_NewStatus(req));
			}
			_user.UpdateMood(req.Owner, _time.DeltaTime);
		}
	}
}
