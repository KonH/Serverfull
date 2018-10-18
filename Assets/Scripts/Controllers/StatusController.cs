using System;
using System.ComponentModel;
using UDBase.Controllers.EventSystem;
using UDBase.Controllers.LogSystem;
using Serverfull.Events;
using Serverfull.Models;
using Zenject;

namespace Serverfull.Controllers {
	public class StatusController : ILogContext, IInitializable, IDisposable {
		readonly IEvent            _event;
		readonly ULogger           _logger;
		readonly FinanceController _finance;

		bool _ended;

		public StatusController(IEvent events, ILog log, FinanceController finance) {
			_event   = events;
			_logger  = log.CreateLogger(this);
			_finance = finance;
		}

		public void Initialize() {
			_finance.State.PropertyChanged += OnBalanceChanged;
		}

		public void Dispose() {
			_finance.State.PropertyChanged -= OnBalanceChanged;
		}

		private void OnBalanceChanged(object sender, PropertyChangedEventArgs e) {
			if ( _finance.Balance < Money.Zero ) {
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
