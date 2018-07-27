namespace Serverfull.Events {
	public struct Tutorial_Complete {
		public string Name { get; }

		public Tutorial_Complete(string name) {
			Name = name;
		}
	}
}