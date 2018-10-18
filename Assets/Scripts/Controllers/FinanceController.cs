using UDBase.Controllers.LogSystem;
using UDBase.Controllers.SaveSystem;
using Serverfull.Common;
using Serverfull.Models;

namespace Serverfull.Controllers {
	public class FinanceController : ILogContext, ISavable {
		public FinanceModel State   { get; private set; }
		public Money        Balance => State.Balance;

		readonly ULogger      _log;
		readonly GameSettings _settings;

		public FinanceController(ILog log, GameSettings settings) {
			_log      = log.CreateLogger(this);
			_settings = settings;
		}

		public void Load(ISave save) {
			State = save.GetNode<FinanceModel>(false);
			if ( State == null ) {
				State = new FinanceModel(new Money(_settings.StartMoney));
			}
		}

		public void Save(ISave save) {
			save.SaveNode(State);
		}

		public void Spend(Money money) {
			if ( _settings.NoExpenses ) {
				return;
			}
			State.Balance -= money;
			_log.MessageFormat("Spend: {0} => {1}", money, Balance);
		}

		public void Add(Money money) {
			State.Balance += money;
			_log.MessageFormat("Add: {0} => {1}", money, Balance);
		}
	}
}
