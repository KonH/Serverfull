using Serverfull.Controllers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Serverfull.UI.Game {
	[RequireComponent(typeof(Text))]
	public class TimeText : MonoBehaviour {
		public string Format = "MM/dd/yyyy HH:mm";

		TimeController _time;
		Text           _text;

		int  _prevValue = -1;

		[Inject]
		public void Init(TimeController time) {
			_time = time;
		}

		void Start() {
			_text = GetComponent<Text>();
		}

		void Update() {
			if ( _time.GameTime.Second != _prevValue ) {
				_text.text = _time.GameTime.ToString(Format);
				_prevValue = _time.GameTime.Second;
			}
		}
	}
}
