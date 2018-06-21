using System;

namespace Serverfull.Game {
	[Serializable]
	public class GameSettings {
		public float TimeScale            = 1.0f;
		public float GameTimeScale        = 1.0f;
		public float NetworkTime          = 1.0f;
		public float ProcessTime          = 1.0f;
		public float MoodDecrease         = 1.0f;
		public int   ServerNetwork        = 1;
		public int   ServerCPU            = 1;
		public int   ServerRAM            = 1;
		public int   ServerMaintenance    = 1;
	}
}
