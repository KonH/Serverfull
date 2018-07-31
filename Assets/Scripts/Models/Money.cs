using System;

namespace Serverfull.Models {
	public struct Money : IComparable<Money> {
		public static Money Zero => new Money(0);

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

		public static bool operator >(Money a, Money b) {
			return a.Value > b.Value;
		}

		public static bool operator >=(Money a, Money b) {
			return a.Value >= b.Value;
		}

		public static bool operator <(Money a, Money b) {
			return a.Value < b.Value;
		}

		public static bool operator <=(Money a, Money b) {
			return a.Value <= b.Value;
		}

		public override string ToString() {
			return string.Format("${0}", Value);
		}

		public override bool Equals(object obj) {
			if ( obj is Money ) {
				var m = (Money)obj;
				return Value.Equals(m.Value);
			}
			return false;
		}

		public override int GetHashCode() {
			return Value.GetHashCode();
		}

		public int CompareTo(Money other) {
			return Value.CompareTo(other.Value);
		}
	}
}
