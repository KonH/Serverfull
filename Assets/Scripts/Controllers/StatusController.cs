using System;
using UDBase.Controllers.EventSystem;
using UDBase.Controllers.LogSystem;
using Serverfull.Events;
using Serverfull.Models;
using Zenject;

namespace Serverfull.Controllers {
	public class StatusController : ILogContext, IInitializable, IDisposable {
		readonly IEvent  _event;
		readonly ULogger _logger;

		bool _ended;

		public StatusController(IEvent events, ILog log) {
			_event  = events;
			_logger = log.CreateLogger(this);
		}

		public void Initialize() {
			_event.Subscribe<Balance_Changed>(this, OnBalanceChanged);
		}

		public void Dispose() {
			_event.Unsubscribe<Balance_Changed>(OnBalanceChanged);
		}

		void OnBalanceChanged(Balance_Changed e) {
			if ( e.NewBalance < Money.Zero ) {
				RaiseGameEnd();
			}
		}

		void RaiseGameEnd() {
			if ( _ended ) {
				return;
			}
			_ended = true;
			_logger.Message("Game ended");
			_event.Fire(new Status_GameEnd());
		}
	}
}
