using UnityEngine;

public class User {
	public float Mood { get; private set; }

	public User() {
		Mood = 1.0f;
	}

	public void UpdateMood(float inc) {
		Mood = Mathf.Max(Mood + inc, 0);
	}
}
