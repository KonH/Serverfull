using System.Collections.Generic;
using UDBase.Controllers.EventSystem;
using UDBase.Controllers.LogSystem;
using Zenject;

public class RequestController : ITickable, ILogContext {
	readonly ULogger _log;
	readonly GameSettings _settings;
	readonly TimeController _time;
	readonly IEvent _events;

	float _spawnTimer = 0.0f;

	List<Request> _requests = new List<Request>(); 

	public RequestController(ILog log, GameSettings settings, TimeController time, IEvent events) {
		_log = log.CreateLogger(this);
		_settings = settings;
		_time = time;
		_events = events;
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

	void InitiateRequest() {
		var req = new Request(_requests.Count);
		_log.MessageFormat("InitiateRequest: {0}", req);
		_requests.Add(req);
	}
	
	void UpdateRequests() {
		var timeDelta = _time.DeltaTime;
		foreach ( var req in _requests ) {
			if ( !req.IsFinished && req.UpdateProgress(timeDelta) ) {
				var status = req.Status;
				_events.Fire(new RequestReady(req));
				if ( req.Status != status ) {
					_events.Fire(new RequestNewStatus(req));
				}
			}
		}
	}
}
