namespace Serverfull.Models {
	public struct RequestId {
		static long _counter = 0;

		public static RequestId Create() {
			_counter++;
			return new RequestId(_counter);
		}

		public long Value { get; }

		RequestId(long value) {
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
