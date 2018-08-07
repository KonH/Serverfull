using Serverfull.Common;
using Serverfull.Models;

namespace Serverfull.Controllers {
	public class UserController {
		readonly GameRules _rules;

		public UserController(GameRules rules) {
			_rules = rules;
		}

		public User CreateUser(ClientId owner) {
			return new User(owner);
		}

		public void UpdateMood(User user, float deltaTime) {
			var value = _rules.CalculateUserMoodChange(deltaTime);
			user.UpdateMood(value);
		}

		public void OnRequestFailed(User user) {
			user.UpdateMood(-user.Mood);
		}
	}
}