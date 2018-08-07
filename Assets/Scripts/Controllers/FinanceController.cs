using UDBase.Controllers.LogSystem;
using UDBase.Controllers.EventSystem;
using Serverfull.Common;
using Serverfull.Models;
using Serverfull.Events;

namespace Serverfull.Controllers {
	public class FinanceController : ILogContext {
		public Money Balance { get; private set; }

		readonly ULogger      _log;
		readonly IEvent       _event;
		readonly GameSettings _settings;

		public FinanceController(ILog log, IEvent events, GameSettings settings) {
			_log      = log.CreateLogger(this);
			_event    = events;
			_settings = settings;

			Balance = new Money(_settings.StartMoney);
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
	}
}
