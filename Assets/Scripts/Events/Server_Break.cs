using Serverfull.Models;

namespace Serverfull.Events {
	public struct Server_Break {
		public ServerId Id { get; }

		public Server_Break(ServerId id) {
			Id = id;
		}
	}
}