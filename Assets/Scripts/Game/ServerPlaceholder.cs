using UnityEngine;

namespace Serverfull.Game {
	public class ServerPlaceholder : MonoBehaviour {
		public GameObject ValidState;
		public GameObject InvalidState;

		public void Show() {
			gameObject.SetActive(true);
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
