using System;

namespace Serverfull.Game {
	[Serializable]
	public class GameSettings {
		public float TimeScale;
		public float MoodDecrease;
		public int   ServerNetwork;
		public int   ServerCPU;
		public int   ServerRAM;
		public int   ServerMaintenance;
		public float NetworkToTime;
		public float CpuToTime;
	}
}
