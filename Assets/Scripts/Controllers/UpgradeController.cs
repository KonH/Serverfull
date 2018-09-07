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
		readonly List<UpgradeLevelInfo> _clientLevels;
		readonly List<UpgradeLevelInfo> _customLevels;

		public UpgradeController(ILog log, GameSettings settings, ServerController server, FinanceController finance) {
			_log     = log.CreateLogger(this);
			_server  = server;
			_finance = finance;

			_clientLevels = LoadLevels(settings.ClientUpgrades);
			_customLevels = LoadLevels(settings.CustomUpgrades);
		}

		List<UpgradeLevelInfo> LoadLevels(List<ServerUpgrade> levels) {
			return levels.Select(u => new UpgradeLevelInfo(new Money(u.Price), new Money(u.Maintanance), u.CPU, u.RAM, u.Network)).ToList();
		}

		UpgradeLevelInfo GetUpgradeLevelInfo(List<UpgradeLevelInfo> levels, int levelIndex) {
			if ( (levelIndex >= 0) && (levelIndex < levels.Count) ) {
				return levels[levelIndex];
			}
			return null;
		}

		public UpgradeLevelInfo GetUpgradeLevelInfo(ServerType type, int levelIndex) {
			return GetUpgradeLevelInfo(type == ServerType.Client ? _clientLevels : _customLevels, levelIndex);
		}

		UpgradeLevelInfo GetNextUpgradeInfo(ServerId id) {
			var server = _server.Get(id);
			if ( server != null ) {
				return GetUpgradeLevelInfo(server.Type, server.UpgradeLevel + 1);
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