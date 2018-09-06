using System.Collections.Generic;
using Serverfull.Common;
using Serverfull.Models;

namespace Serverfull.Controllers {
	public class UserController {
		readonly GameRules        _rules;
		readonly ClientController _client;

		public UserController(GameRules rules, ClientController client) {
			_rules  = rules;
			_client = client;
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

		public List<ServerType> GetAdditionalServers(User user) {
			return (user != null) ? _client.Get(user.Client)?.AdditionalServers : null;
		}
	}
}