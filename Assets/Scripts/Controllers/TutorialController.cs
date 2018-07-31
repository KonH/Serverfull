﻿using System;
using System.Collections.Generic;
using UDBase.Controllers.EventSystem;
using Serverfull.Events;
using Zenject;

namespace Serverfull.Controllers {
	public class TutorialController : IInitializable, IDisposable {
		readonly IEvent         _event;
		readonly TimeController _time;

		bool            _isActive  = false;
		Queue<string>   _tutorials = new Queue<string>();
		HashSet<string> _completed = new HashSet<string>();

		public TutorialController(IEvent events, TimeController time) {
			_event = events;
			_time  = time;
		}

		public void Initialize() {
			_event.Subscribe<Tutorial_Complete>(this, OnTutorialComplete);
			_event.Subscribe<Time_Started>     (this, OnTimeStarted);
			_event.Subscribe<Server_New>       (this, OnServerNew);
		}

		public void Dispose() {
			_event.Unsubscribe<Tutorial_Complete>(OnTutorialComplete);
			_event.Unsubscribe<Time_Started>     (OnTimeStarted);
			_event.Unsubscribe<Server_New>       (OnServerNew);
		}

		void OnTutorialComplete(Tutorial_Complete e) {
			_completed.Add(e.Name);
			_isActive = false;
			_time.Resume();
			TriggerNextTutorial();
		}

		void OnTimeStarted(Time_Started e) {
			StartTutorials("Clients", "FirstServer");
		}

		void OnServerNew(Server_New e) {
			StartTutorials("ServerDetails", "ServerUpgrade", "DragClient");
		}

		void StartTutorial(string tutorial) {
			if ( _completed.Contains(tutorial) ) {
				return;
			}
			_tutorials.Enqueue(tutorial);
			TriggerNextTutorial();
		}

		void StartTutorials(params string[] tutorials) {
			foreach ( var tutor in tutorials ) {
				StartTutorial(tutor);
			}
		}

		void TriggerNextTutorial() {
			if ( !_isActive ) {
				if ( _tutorials.Count > 0 ) {
					_isActive = true;
					_time.Pause();
					var tutName = _tutorials.Dequeue();
					_event.Fire(new Tutorial_New(tutName));
				}
			}
		}
	}
}