namespace Serverfull.Models {
	public class Client {
		public ClientId Id       { get; }
		public Money    Income   { get; }
		public int      UserRate { get; }
		public float    Mood     { get; private set; } = 1.0f;

		public Client(ClientId id, Money income, int userRate) {
			Id       = id;
			Income   = income;
			UserRate = userRate;
		}
	}
}
