﻿using System;
using UDBase.Controllers.LogSystem;
using UDBase.Controllers.EventSystem;
using Serverfull.Models;
using Serverfull.Events;
using Zenject;

namespace Serverfull.Controllers {
	public class FinanceController : ILogContext, IInitializable, IDisposable {
		public Money Balance { get; private set; }

		readonly ULogger          _log;
		readonly IEvent           _event;
		readonly ServerController _server;

		public FinanceController(ILog log, IEvent events, ServerController server) {
			_log    = log.CreateLogger(this);
			_event  = events;
			_server = server;
		}

		public void Initialize() {
			_event.Subscribe<Time_NewGameHour>(this, OnNewHour);
		}

		public void Dispose() {
			_event.Unsubscribe<Time_NewGameHour>(OnNewHour);
		}

		void OnNewHour(Time_NewGameHour e) {
			Balance -= _server.GetTotalMaintenance();
			_log.MessageFormat("New balance: {0}", Balance);
		}
	}
}