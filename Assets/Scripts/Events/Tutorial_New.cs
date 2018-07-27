namespace Serverfull.Events {
	public struct Tutorial_New {
		public string Name { get; }

		public Tutorial_New(string name) {
			Name = name;
		}
	}
}