using System.Collections.Generic;

namespace Serverfull.Models {
	public class Server {
		public const string Network = "Network";
		public const string CPU     = "CPU";
		public const string RAM     = "RAM";

		public ServerId                Id          { get; }
		public float                   NetworkTime { get; }
		public float                   ProcessTime { get; }
		public Money                   Maintenance { get; }
		public Dictionary<string, int> Resources   { get; private set; }

		public Server(ServerId id, Money maintenance, float networkTime, float processTime, Dictionary<string, int> resources) {
			Id          = id;
			Maintenance = maintenance;
			NetworkTime = networkTime;
			ProcessTime = processTime;
			Resources   = resources;
		}

		public override string ToString() {
			return string.Format("[{0}] Network: {1}, CPU: {2}, RAM: {3}", Id, Resources[Network], Resources[CPU], Resources[RAM]);
		}

		public override int GetHashCode() {
			return Id.GetHashCode();
		}
	}
}
