﻿using System;
using UDBase.Controllers.LogSystem;
using UDBase.Controllers.EventSystem;
using Serverfull.Common;
using Serverfull.Models;
using Serverfull.Events;
using Zenject;

namespace Serverfull.Controllers {
	public class FinanceController : ILogContext, IInitializable, IDisposable {
		public Money Balance { get; private set; }

		readonly ULogger          _log;
		readonly IEvent           _event;
		readonly GameSettings     _settings;
		readonly ServerController _server;
		readonly ClientController _client;

		public FinanceController(ILog log, IEvent events, GameSettings settings, ServerController server, ClientController client) {
			_log      = log.CreateLogger(this);
			_event    = events;
			_settings = settings;
			_server   = server;
			_client   = client;

			Balance = new Money(_settings.StartMoney);
		}

		public void Initialize() {
			_event.Subscribe<Time_NewGameHour>(this, OnNewHour);
		}

		public void Dispose() {
			_event.Unsubscribe<Time_NewGameHour>(OnNewHour);
		}

		void RaiseUpdateEvent() {
			_event.Fire(new Balance_Changed(Balance));
		}

		public void Spend(Money money) {
			if ( _settings.NoExpenses ) {
				return;
			}
			Balance -= money;
			_log.MessageFormat("Spend: {0} => {1}", money, Balance);
			RaiseUpdateEvent();
		}

		public void Add(Money money) {
			Balance += money;
			_log.MessageFormat("Add: {0} => {1}", money, Balance);
			RaiseUpdateEvent();
		}

		void OnNewHour(Time_NewGameHour e) {
			Spend(_server.GetTotalMaintenance());
			Add(_client.GetTotalIncome());
		}
	}
}
