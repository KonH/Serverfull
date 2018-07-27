using UnityEngine;

namespace Serverfull.Models {
	public class User {
		public ClientId Client { get; }
		public float    Mood   { get; private set; } = 100.0f;

		public float NormalizedMood => Mood / 100.0f;

		public User(ClientId client) {
			Client = client;
		}

		public void UpdateMood(float inc) {
			Mood = Mathf.Max(Mood + inc, 0);
		}
	}
}
