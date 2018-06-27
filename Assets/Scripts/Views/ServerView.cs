using UnityEngine;
using Serverfull.Models;

namespace Serverfull.Views {
	public class ServerView : MonoBehaviour {
		public Transform Center;

		public ServerId Id { get; private set; }

		public void Init(ServerId id) {
			Id = id;
		}
	}
}
