using Serverfull.Models;

namespace Serverfull.Events {
	public struct UI_ServerSelected {
		public ServerId Id { get; }

		public UI_ServerSelected(ServerId id) {
			Id = id;
		}
	}
}
