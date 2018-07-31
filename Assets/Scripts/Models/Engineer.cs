namespace Serverfull.Models {
	public class Engineer {
		public EngineerId Id      { get; }
		public float      FixTime { get; }
		public Money      Price   { get; }
		public Money      Salary  { get; }
		public bool       Hired   { get; private set; }

		public Engineer(EngineerId id, float fixTime, Money price, Money salary, bool hired) {
			Id      = id;
			FixTime = fixTime;
			Price   = price;
			Salary  = salary;
			Hired   = hired;
		}

		public void Hire() {
			Hired = true;
		}
	}
}
