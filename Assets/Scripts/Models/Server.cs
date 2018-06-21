using System.Collections.Generic;

namespace Serverfull.Models {
	public class Server {
		public const string Network = "Network";
		public const string CPU     = "CPU";
		public const string RAM     = "RAM";

		public class Resource {
			public int   Max  { get; }
			public int   Free { get; set; }

			public float NormalizedFree => (float)Free / Max;

			public Resource(int value) {
				Max  = value;
				Free = value;
			}
		}

		public ServerId                     Id          { get; }
		public float                        NetworkTime { get; }
		public float                        ProcessTime { get; }
		public Money                        Maintenance { get; }
		public Dictionary<string, Resource> Resources   { get; private set; } = new Dictionary<string, Resource>();
		public List<ClientId>               Clients     { get; private set; } = new List<ClientId>();

		public Server(ServerId id, Money maintenance, float networkTime, float processTime, Dictionary<string, int> resources) {
			Id          = id;
			Maintenance = maintenance;
			NetworkTime = networkTime;
			ProcessTime = processTime;
			foreach ( var res in resources ) {
				Resources.Add(res.Key, new Resource(res.Value));
			}
		}

		public override string ToString() {
			return string.Format("[{0}] Network: {1}, CPU: {2}, RAM: {3}", Id, Resources[Network], Resources[CPU], Resources[RAM]);
		}

		public override int GetHashCode() {
			return Id.GetHashCode();
		}
	}
}
