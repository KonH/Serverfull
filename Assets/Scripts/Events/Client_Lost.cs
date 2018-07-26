using Serverfull.Models;

namespace Serverfull.Events {
	public struct Client_Lost {
		public ClientId Id { get; }

		public Client_Lost(ClientId id) {
			Id = id;
		}
	}
}
