using System;
using Serverfull.Models;

namespace Serverfull.Controllers {
	public class ServerBuildController {
		readonly ServerController  _server;
		readonly UpgradeController _upgrade;
		readonly FinanceController _finance;

		public ServerBuildController(ServerController server, UpgradeController upgrade, FinanceController finance) {
			_server  = server;
			_upgrade = upgrade;
			_finance = finance;
		}

		public void AddServer(ServerType type, int x, int y) {
			var upgradeLevel = 0;
			var levelInfo = _upgrade.GetUpgradeLevelInfo(upgradeLevel);
			if ( _finance.Balance > levelInfo.Price ) {
				var server = new Server(ServerId.Create(), type, x, y, upgradeLevel, levelInfo.Maintenance, levelInfo.Network, levelInfo.CPU, levelInfo.RAM);
				_server.Add(server);
				_finance.Spend(levelInfo.Price);
			}
		}

		public bool IsValidPosition(int x, int y) {
			var servers = _server.All;
			foreach ( var server in servers ) {
				var dx = Math.Abs(server.PosX - x);
				var dy = Math.Abs(server.PosY - y);
				if ( (dx <= 1) && (dy <= 1) ) {
					return false;
				}
			}
			return true;
		}
	}
}
