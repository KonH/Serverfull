using System.Collections.Generic;

namespace Serverfull.Models {
	public class Server {
		public class Resource {
			public int   Free { get; set; }
			public int   Max  { get; }

			public float NormalizedFree => (float)Free / Max;
			public int Busy             => Max - Free;

			public Resource(int free, int max) {
				Free = free;
				Max  = max;
			}

			public Resource(int value) : this(value, value) { }

		}

		public ServerId       Id           { get; }
		public int            PosX         { get; }
		public int            PosY         { get; }
		public int            UpgradeLevel { get; private set; }
		public Money          Maintenance  { get; private set; }
		public Resource       Network      { get; private set; }
		public Resource       CPU          { get; private set; }
		public Resource       RAM          { get; private set; }
		public List<ClientId> Clients      { get; private set; } = new List<ClientId>();

		public Server(ServerId id, int x, int y, int upgradeLevel, Money maintenance, int network, int cpu, int ram) {
			Id           = id;
			PosX         = x;
			PosY         = y;
			UpgradeLevel = upgradeLevel;
			Maintenance  = maintenance;
			Network      = new Resource(network);
			CPU          = new Resource(cpu);
			RAM          = new Resource(ram);

		}

		public void Upgrade(int level, int network, int cpu, int ram) {
			UpgradeLevel = level;
			Network      = new Resource(network - Network.Busy, network);
			CPU          = new Resource(cpu     - CPU.Busy,     cpu);
			RAM          = new Resource(ram     - RAM.Busy,     ram);
		}

		public override string ToString() {
			return string.Format("[{0}] Network: {1}, CPU: {2}, RAM: {3}", Id, Network, CPU, RAM);
		}

		public override int GetHashCode() {
			return Id.GetHashCode();
		}
	}
}
