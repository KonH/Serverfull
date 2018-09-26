using UnityEngine;
using UnityEngine.UI;
using Serverfull.Controllers;
using Zenject;

namespace Serverfull.UI.Game {
	[RequireComponent(typeof(Button))]
	public class SaveGameButton : MonoBehaviour {
		SaveController _save;

		[Inject]
		void Init(SaveController save) {
			_save = save;
			GetComponent<Button>().onClick.AddListener(Save);
		}

		void Save() {
			_save.Save();
		}
	}
}
