namespace Serverfull.Models {
	public struct ClientId {
		public static ClientId Empty => new ClientId(string.Empty);

		public string Name { get; }

		public bool IsEmpty => string.IsNullOrEmpty(Name);

		public ClientId(string name) {
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