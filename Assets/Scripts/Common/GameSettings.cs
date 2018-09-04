using System;
using System.Collections.Generic;
using UnityEngine;

namespace Serverfull.Common {
	[Serializable]
	public class GameSettings {
		public bool  WithTutorials;
		public float TimeScale;
		public float UserMoodChange;
		public float ClientMoodChange;
		public float ClientMoodBorder;
		public int   StartMoney;
		public bool  NoExpenses;

		[Header("Servers")]
		public float NetworkToTime;
		public float CpuToTime;
		public float BreakChance;
		public List<ServerUpgrade> Upgrades;

		[Header("Clients")]
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
	}
}
