namespace Serverfull.Models {
	public class Client {
		public ClientId Id            { get; }
		public Money    Income        { get; }
		public int      UserRate      { get; }
		public int      WantedNetwork { get; }
		public int      WantedCPU     { get; }
		public int      WantedRAM     { get; }
		public float    Mood          { get; private set; } = 1.0f;

		public Client(ClientId id, Money income, int userRate, int wantedNetwork, int wantedCpu, int wantedRam) {
			Id            = id;
			Income        = income;
			UserRate      = userRate;
			WantedNetwork = wantedNetwork;
			WantedCPU     = wantedCpu;
			WantedRAM     = wantedRam;
		}
	}
}
