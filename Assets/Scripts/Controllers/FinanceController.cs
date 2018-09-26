using UDBase.Controllers.LogSystem;
using UDBase.Controllers.SaveSystem;
using UDBase.Controllers.EventSystem;
using Serverfull.Common;
using Serverfull.Models;
using Serverfull.Events;

namespace Serverfull.Controllers {
	public class FinanceController : ILogContext, ISavable {
		public class State : ISaveSource {
			public Money Balance;
		}

		public Money Balance => _state.Balance;

		State _state;

		readonly ULogger      _log;
		readonly IEvent       _event;
		readonly GameSettings _settings;

		public FinanceController(ILog log, IEvent events, GameSettings settings) {
			_log      = log.CreateLogger(this);
			_event    = events;
			_settings = settings;
		}

		public void Load(ISave save) {
			_state = save.GetNode<State>(false);
			if ( _state == null ) {
				_state = new State {
					Balance = new Money(_settings.StartMoney)
				};
			}
		}

		public void Save(ISave save) {
			save.SaveNode(_state);
		}

		void RaiseUpdateEvent() {
			_event.Fire(new Balance_Changed(Balance));
		}

		public void Spend(Money money) {
			if ( _settings.NoExpenses ) {
				return;
			}
			_state.Balance -= money;
			_log.MessageFormat("Spend: {0} => {1}", money, Balance);
			RaiseUpdateEvent();
		}

		public void Add(Money money) {
			_state.Balance += money;
			_log.MessageFormat("Add: {0} => {1}", money, Balance);
			RaiseUpdateEvent();
		}
	}
}
