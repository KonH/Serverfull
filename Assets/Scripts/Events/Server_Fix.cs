using Serverfull.Models;

namespace Serverfull.Events {
	public struct Server_Fix {
		public ServerId Id { get; }

		public Server_Fix(ServerId id) {
			Id = id;
		}
	}
}