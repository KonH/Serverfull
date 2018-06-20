using UnityEngine;
using Serverfull.Models;

namespace Serverfull.Views {
	public class ServerView : MonoBehaviour {
		public ServerId Id => new ServerId(ServerId); 

		[SerializeField]
		int ServerId;
	}
}
