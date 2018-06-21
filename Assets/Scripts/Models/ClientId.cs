namespace Serverfull.Models {
	public struct ClientId {
		public string Name { get; }

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