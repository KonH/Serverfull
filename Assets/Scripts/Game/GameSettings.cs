using System;

namespace Serverfull.Game {
	[Serializable]
	public class GameSettings {
		public float TimeScale;
		public float UserMoodChange;
		public float ClientMoodChange;
		public float NetworkToTime;
		public float CpuToTime;
		public int   StartMoney;
	}
}
