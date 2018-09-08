using System.Collections.Generic;
using Serverfull.Common;
using Serverfull.Models;

namespace Serverfull.Controllers {
	public class UserController {
		readonly ClientController _client;

		public UserController(ClientController client) {
			_client = client;
		}

		public User CreateUser(ClientId owner) {
			return new User(owner);
		}

		public void OnRequestFailed(User user) {
			user.UpdateMood(-user.Mood);
		}

		public List<ServerType> GetAdditionalServers(User user) {
			return (user != null) ? _client.Get(user.Client)?.AdditionalServers : null;
		}
	}
}