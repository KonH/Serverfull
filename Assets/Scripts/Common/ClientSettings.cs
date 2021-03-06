﻿using System;
using UDBase.Helpers;

namespace Serverfull.Common {
	[Serializable]
	public class ClientSettings {
		public string Difficulty;
		public IntRange UserRate;
		public IntRange Network;
		public IntRange RAM;
		public IntRange CPU;
		public IntRange PlusServers;
		public IntRange Income;
		public float    Chance;
	}
}