using System;
using UDBase.Helpers;

namespace Serverfull.Common {
	[Serializable]
	public class EngineerSettings {
		public string     Level;
		public IntRange   Price;
		public IntRange   Salary;
		public FloatRange FixTime;
		public float      Chance;
	}
}