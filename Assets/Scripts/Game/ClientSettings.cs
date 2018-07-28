using System;
using UDBase.Helpers;

namespace Serverfull.Game {
	[Serializable]
	public class ClientSettings {
		public string Difficulty;
		public IntRange UserRate;
		public IntRange Network;
		public IntRange RAM;
		public IntRange CPU;
		public IntRange Income;
		public float    Chance;
	}
}