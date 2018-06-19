public class UserController {
	GameSettings _settings;

	public UserController(GameSettings settings) {
		_settings = settings;
	}

	public User CreateUser() {
		return new User();
	}

	public void UpdateMood(User user, float deltaTime) {
		user.UpdateMood(-deltaTime * _settings.MoodDecrease);
	}

	public void OnRequestFailed(User user) {
		user.UpdateMood(-user.Mood);
	}
}
