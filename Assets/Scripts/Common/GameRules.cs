using UnityEngine;
using Serverfull.Models;

namespace Serverfull.Common {
	public class GameRules {
		readonly GameSettings _settings;

		public GameRules(GameSettings settings) {
			_settings = settings;
		}

		public float GetProcessTime(Server server) {
			var cpu = server.CPU.Max;
			return (_settings.CpuToTime * 1000) / cpu;
		}

		public float GetNetworkTime(Server server) {
			var network = server.Network.Max;
			return (_settings.NetworkToTime * 1000) / network;
		}

		public float CalculateClientMoodChange(float userMood) {
			var value = (userMood > 0 ? 1.0f : -1.0f) * _settings.ClientMoodChange;
			return value;
		}

		public float GetBreakChance(float deltaTime) {
			return deltaTime * _settings.BreakChance;
		}
	}
}
