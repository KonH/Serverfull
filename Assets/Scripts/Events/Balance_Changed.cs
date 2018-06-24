using Serverfull.Models;

namespace Serverfull.Events {
	public struct Balance_Changed {
		public Money NewBalance { get; }

		public Balance_Changed(Money newBalance) {
			NewBalance = newBalance;
		}
	}
}