using Serverfull.Models;
using UnityEngine;

namespace Serverfull.Game {
	public class ServerPlaceholder : MonoBehaviour {
		public GameObject ValidState;
		public GameObject InvalidState;
		
		public ServerType Type { get; private set; }

		public void Show(ServerType type) {
			gameObject.SetActive(true);
			Type = type;
		}

		public void Hide() {
			gameObject.SetActive(false);
		}

		public void UpdateState(Vector2Int pos, bool valid) {
			transform.position = new Vector3(pos.x, 0, pos.y);
			ValidState.SetActive  (valid);
			InvalidState.SetActive(!valid);
		}
	}
}
