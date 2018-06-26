using System.Collections.Generic;
using UDBase.Controllers.LogSystem;
using Serverfull.Models;

namespace Serverfull.Controllers {
	public class UpgradeController : ILogContext {
		readonly ULogger           _log;
		readonly ServerController  _server;
		readonly FinanceController _finance;

		List<UpgradeLevelInfo> _levels = new List<UpgradeLevelInfo> {
			new UpgradeLevelInfo(new Money(0), new Money(1),  1, 1, 1),
			new UpgradeLevelInfo(new Money(1), new Money(2),  5, 5, 5),
			new UpgradeLevelInfo(new Money(5), new Money(3), 25, 25, 25),
			new UpgradeLevelInfo(new Money(10), new Money(4), 100, 100, 100)
		};

		public UpgradeController(ILog log, ServerController server, FinanceController finance) {
			_log     = log.CreateLogger(this);
			_server  = server;
			_finance = finance;
		}

		public UpgradeLevelInfo GetUpgradeLevelInfo(int levelIndex) {
			if ( (levelIndex >= 0) && (levelIndex < _levels.Count) ) {
				return _levels[levelIndex];
			}
			return null;
		}

		UpgradeLevelInfo GetNextUpgradeInfo(ServerId id) {
			var server = _server.Get(id);
			if ( server != null ) {
				return GetUpgradeLevelInfo(server.UpgradeLevel + 1);
			}
			return null;
		}

		public bool CanUpgrade(ServerId id) {
			var nextLevel = GetNextUpgradeInfo(id);
			return ( (nextLevel != null) && (_finance.Balance > nextLevel.Price) );
		}

		public void Upgrade(ServerId id) {
			if ( CanUpgrade(id) ) {
				var level = GetNextUpgradeInfo(id);
				_finance.Spend(level.Price);
				var server = _server.Get(id);
				server.Upgrade(server.UpgradeLevel + 1, level.Network, level.CPU, level.RAM);
				_log.MessageFormat("Upgrade server {0} to level {1}.", server.Id, server.UpgradeLevel);
			}
		}
	}
}