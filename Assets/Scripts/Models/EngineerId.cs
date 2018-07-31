namespace Serverfull.Models {
	public struct EngineerId {
		public string Name { get; }

		public bool IsEmpty => string.IsNullOrEmpty(Name);

		public EngineerId(string name) {
			Name = name;
		}

		public override int GetHashCode() {
			return Name.GetHashCode();
		}

		public override string ToString() {
			return Name;
		}
	}
}