using Serverfull.Models;

namespace Serverfull.Events {
	public struct Engineer_New {
		public EngineerId Id { get; }

		public Engineer_New(EngineerId id) {
			Id = id;
		}
	}
}
