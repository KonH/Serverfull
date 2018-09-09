using System;
using System.Collections.Generic;
using UnityEngine;

namespace Serverfull.Common {
	[Serializable]
	public class GameSettings {
		public bool  WithTutorials;
		public float TimeScale;
		public int   StartMoney;
		public bool  NoExpenses;

		[Header("Servers")]
		public float NetworkToTime;
		public float CpuToTime;
		public float BreakChance;
		public List<ServerUpgrade> ClientUpgrades;
		public List<ServerUpgrade> CustomUpgrades;

		[Header("Clients")]
		public float                ClientMoodChange;
		public int                  ClientSpawnInterval;
		public int                  ClientsPerSpawn;
		public int                  FirstClientSpawn;
		public List<string>         ClientNames;
		public List<ClientSettings> ClientSetups;

		[Header("Engineers")]
		public int                    EngineerSpawnInterval;
		public int                    EngineersPerSpawn;
		public int                    FirstEngineerSpawn;
		public List<string>           EngineerNames;
		public List<EngineerSettings> EngineerSetups;

		[Header("Requests")]
		public float MinRequestTimeCoeff;
		public float MaxRequestTimeCoeff;
	}
}
