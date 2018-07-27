using UnityEngine;

namespace Serverfull.Models {
	public class Client {
		public ClientId Id            { get; }
		public Money    Income        { get; }
		public string   Difficulty    { get; }
		public int      UserRate      { get; }
		public int      WantedNetwork { get; }
		public int      WantedCPU     { get; }
		public int      WantedRAM     { get; }
		public float    Mood          { get; private set; } = 100.0f;

		public float NormalizedMood => Mood / 100.0f;

		public Client(ClientId id, Money income, string difficulty, int userRate, int wantedNetwork, int wantedCpu, int wantedRam) {
			Id            = id;
			Income        = income;
			Difficulty    = difficulty;
			UserRate      = userRate;
			WantedNetwork = wantedNetwork;
			WantedCPU     = wantedCpu;
			WantedRAM     = wantedRam;
		}

		public void UpdateMood(float change) {
			Mood = Mathf.Clamp(Mood + change, 0, 100.0f);
		}
	}
}
