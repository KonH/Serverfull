using Serverfull.Models;

namespace Serverfull.Events {
	public struct Engineer_Hired {
		public EngineerId Id { get; }

		public Engineer_Hired(EngineerId id) {
			Id = id;
		}
	}
}
