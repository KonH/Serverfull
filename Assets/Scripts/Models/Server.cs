using System.Collections.Generic;

namespace Serverfull.Models {
	public class Server {
		public const string Network = "Network";
		public const string CPU     = "CPU";
		public const string RAM     = "RAM";

		public ServerId                Id          { get; }
		public float                   NetworkTime { get; private set; }
		public float                   ProcessTime { get; private set; }
		public Dictionary<string, int> Resources   { get; private set; }

		public Server(ServerId id, float networkTime, float processTime, Dictionary<string, int> resources) {
			Id          = id;
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
