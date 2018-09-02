using System;
using System.Collections.Generic;
using UnityEngine;

namespace Serverfull.Common {
	[Serializable]
	public class GameSettings {
		public float TimeScale;
		public float UserMoodChange;
		public float ClientMoodChange;
		public float ClientMoodBorder;
		public float NetworkToTime;
		public float CpuToTime;
		public int   StartMoney;
		public float BreakChance;
		public bool  NoExpenses;

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
