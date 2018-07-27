using UnityEngine;
using Serverfull.Models;

namespace Serverfull.Game {
	public class GameRules {
		readonly GameSettings _settings;

		public GameRules(GameSettings settings) {
			_settings = settings;
		}

		public float GetProcessTime(Server server) {
			var cpu = server.CPU.Max;
			return _settings.CpuToTime / Mathf.Sqrt(cpu);
		}

		public float GetNetworkTime(Server server) {
			var network = server.Network.Max;
			return _settings.NetworkToTime / Mathf.Sqrt(network);
		}

		public float CalculateUserMoodChange(float deltaTime) {
			return -deltaTime * _settings.UserMoodChange;
		}

		public float CalculateClientMoodChange(float userMood) {
			var value = (userMood - 50.0f) * _settings.ClientMoodChange;
			return value;
		}
	}
}
