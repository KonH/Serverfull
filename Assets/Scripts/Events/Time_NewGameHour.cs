using System;

namespace Serverfull.Events {
	public struct Time_NewGameHour {
		public DateTime GameTime { get; }

		public Time_NewGameHour(DateTime gameTime) {
			GameTime = gameTime;
		}

		public override string ToString() {
			return string.Format("Time_NewGameHour: {0}", GameTime);
		}
	}
}