using Serverfull.Models;

namespace Serverfull.Controllers {
	public class ServerBuildController {
		readonly ServerController  _server;
		readonly UpgradeController _upgrade;

		public ServerBuildController(ServerController server, UpgradeController upgrade) {
			_server  = server;
			_upgrade = upgrade;

			for ( var i = 0; i < 3; i++ ) {
				AddServer();
			}
		}

		void AddServer() {
			var upgradeLevel = 0;
			var levelInfo = _upgrade.GetUpgradeLevelInfo(upgradeLevel);
			var server = new Server(ServerId.Create(), upgradeLevel, levelInfo.Maintenance, levelInfo.Network, levelInfo.CPU, levelInfo.RAM);
			_server.Add(server);
		}
	}
}
