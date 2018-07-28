using System;
using System.Collections.Generic;

namespace Serverfull.Game {
	[Serializable]
	public class GameSettings {
		public float TimeScale;
		public float UserMoodChange;
		public float ClientMoodChange;
		public float ClientMoodBorder;
		public float NetworkToTime;
		public float CpuToTime;
		public int   StartMoney;
		public int   ClientSpawnInterval;
		public int   ClientsPerSpawn;
		public int   FirstClientSpawn;

		public List<string>         ClientNames;
		public List<ClientSettings> Setups;
	}
}
