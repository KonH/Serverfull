using Serverfull.Models;

namespace Serverfull.Game {
	public class GameRules {
		readonly GameSettings _settings;

		public GameRules(GameSettings settings) {
			_settings = settings;
		}

		public float GetProcessTime(Server server) {
			var cpu = server.Resources[Server.CPU].Max;
			return cpu / _settings.CpuToTime;
		}

		public float GetNetworkTime(Server server) {
			var cpu = server.Resources[Server.Network].Max;
			return cpu / _settings.NetworkToTime;
		}
	}
}
