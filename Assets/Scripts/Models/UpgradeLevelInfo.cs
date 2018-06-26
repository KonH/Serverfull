namespace Serverfull.Models {
	public class UpgradeLevelInfo {
		public Money Price;
		public Money Maintenance;
		public int   Network;
		public int   CPU;
		public int   RAM;

		public UpgradeLevelInfo(Money price, Money maintenance, int network, int cpu, int ram) {
			Price       = price;
			Maintenance = maintenance;
			Network     = network;
			CPU         = cpu;
			RAM         = ram;
		}
	}
}