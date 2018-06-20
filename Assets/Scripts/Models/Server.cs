using System.Collections.Generic;

namespace Serverfull.Models {
	public class Server {
		public const string Network = "Network";
		public const string CPU     = "CPU";
		public const string RAM     = "RAM";

		public float                   NetworkTime { get; private set; }
		public float                   ProcessTime { get; private set; }
		public Dictionary<string, int> Resources   { get; private set; }

		public Server(float networkTime, float processTime, Dictionary<string, int> resources) {
			NetworkTime = networkTime;
			ProcessTime = processTime;
			Resources   = resources;
		}

		public override string ToString() {
			return string.Format("Network: {0}, CPU: {1}, RAM: {2}", Resources[Network], Resources[CPU], Resources[RAM]);
		}
	}
}
