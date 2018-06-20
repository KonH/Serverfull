namespace Serverfull.Models {
	public struct Money {
		public int Value { get; private set; }

		public Money(int value) {
			Value = value;
		}
		
		public static Money operator +(Money a, Money b) {
			return new Money(a.Value + b.Value);
		}

		public static Money operator -(Money a, Money b) {
			return new Money(a.Value - b.Value);
		}

		public override string ToString() {
			return string.Format("${0}", Value);
		}
	}
}
