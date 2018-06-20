namespace Serverfull.Models {
	public struct ServerId {
		static int _counter = 0;

		public static ServerId Create() {
			_counter++;
			return new ServerId(_counter);
		}

		public int Value { get; }

		public ServerId(int value) {
			Value = value;
		}

		public override string ToString() {
			return Value.ToString();
		}

		public override int GetHashCode() {
			return Value.GetHashCode();
		}
	}
}
