using UnityEngine;
using UnityEngine.UI;
using Serverfull.Game;
using Zenject;

namespace Serverfull.UI.Game {
	[RequireComponent(typeof(Button))]
	public class ServerBuildButton : MonoBehaviour {
		public Text Label;

		Button        _button;
		ServerBuilder _builder;

		bool _prevStartPlacement = false;

		[Inject]
		public void Init(ServerBuilder builder) {
			_builder = builder;
			_button  = GetComponent<Button>();
			_button.onClick.AddListener(OnClick);
		}

		void Start() {
			UpdateState();
		}

		void OnClick() {
			_builder.StartPlacement();
		}

		void UpdateState() {
			_button.interactable = _builder.CanStartPlacement;
			Label.text           = string.Format("New server ({0})", _builder.BuildPrice);
			_prevStartPlacement = _builder.CanStartPlacement;
		}

		void Update() {
			if ( _builder.CanStartPlacement != _prevStartPlacement ) {
				UpdateState();
			}
		}
	}
}
