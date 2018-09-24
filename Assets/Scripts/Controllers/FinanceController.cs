using UDBase.Controllers.LogSystem;
using UDBase.Controllers.SaveSystem;
using UDBase.Controllers.EventSystem;
using Serverfull.Common;
using Serverfull.Models;
using Serverfull.Events;

namespace Serverfull.Controllers {
	public class FinanceController : ILogContext {
		public class State : ISaveSource {
			public Money Balance;
		}

		public Money Balance => _state.Balance;

		State _state;

		readonly ULogger      _log;
		readonly ISave        _save;
		readonly IEvent       _event;
		readonly GameSettings _settings;

		public FinanceController(ILog log, ISave save, IEvent events, GameSettings settings) {
			_log      = log.CreateLogger(this);
			_save     = save;
			_event    = events;
			_settings = settings;
			Load();
		}

		void Load() {
			_state = _save.GetNode<State>(false);
			if ( _state == null ) {
				_state = new State {
					Balance = new Money(_settings.StartMoney)
				};
			}
		}

		void Save() {
			_save.SaveNode(_state);
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
			Save();
			RaiseUpdateEvent();
		}

		public void Add(Money money) {
			_state.Balance += money;
			_log.MessageFormat("Add: {0} => {1}", money, Balance);
			Save();
			RaiseUpdateEvent();
		}
	}
}
