using System;
using UDBase.Controllers.EventSystem;
using Zenject;

public class ProcessingController : IInitializable, IDisposable {
	readonly IEvent _events;

	public ProcessingController(IEvent events) {
		_events = events;
	}

	public void Initialize() {
		_events.Subscribe<RequestReady>(this, OnRequestReady);
	}

	public void Dispose() {
		_events.Unsubscribe<RequestReady>(OnRequestReady);
	}

	void OnRequestReady(RequestReady e) {
		if ( e.Status == RequestStatus.Incoming ) {
			e.Request.ToProcessing(e.Request.Target.NetworkTime);
		}
	}
}
