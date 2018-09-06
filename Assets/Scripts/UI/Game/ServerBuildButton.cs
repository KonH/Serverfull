using UnityEngine;
using UnityEngine.UI;
using Serverfull.Game;
using Serverfull.Models;

namespace Serverfull.UI.Game {
	[RequireComponent(typeof(Button))]
	public class ServerBuildButton : MonoBehaviour {
		public Text Label;

		Button        _button;
		ServerBuilder _builder;
		ServerType    _type;

		bool _prevStartPlacement = false;

		public void Init(ServerBuilder builder, ServerType type) {
			_builder = builder;
			_button  = GetComponent<Button>();
			_button.onClick.AddListener(OnClick);
			_type = type;
		}

		void Start() {
			if ( !_builder ) {
				return;
			}
			UpdateState();
		}

		void OnClick() {
			_builder.StartPlacement(_type);
		}

		void UpdateState() {
			_button.interactable = _builder.CanStartPlacement;
			Label.text           = string.Format("{0} Server ({1}, {2}/h)", _type, _builder.BuildPrice, _builder.Maintenance);
			_prevStartPlacement = _builder.CanStartPlacement;
		}

		void Update() {
			if ( _builder.CanStartPlacement != _prevStartPlacement ) {
				UpdateState();
			}
		}
	}
}
