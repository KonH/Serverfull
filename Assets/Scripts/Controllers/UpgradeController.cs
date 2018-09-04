using System.Linq;
using System.Collections.Generic;
using UDBase.Controllers.LogSystem;
using Serverfull.Models;
using Serverfull.Common;

namespace Serverfull.Controllers {
	public class UpgradeController : ILogContext {
		readonly ULogger                _log;
		readonly ServerController       _server;
		readonly FinanceController      _finance;
		readonly List<UpgradeLevelInfo> _levels;

		public UpgradeController(ILog log, GameSettings settings, ServerController server, FinanceController finance) {
			_log     = log.CreateLogger(this);
			_levels  = LoadLevels(settings);
			_server  = server;
			_finance = finance;
		}

		List<UpgradeLevelInfo> LoadLevels(GameSettings settings) {
			return settings.Upgrades.Select(u => new UpgradeLevelInfo(new Money(u.Price), new Money(u.Maintanance), u.CPU, u.RAM, u.Network)).ToList();
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