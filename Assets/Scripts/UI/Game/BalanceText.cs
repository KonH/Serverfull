using UnityEngine;
using UnityEngine.UI;
using UDBase.Controllers.EventSystem;
using Serverfull.Events;
using Zenject;

namespace Serverfull.UI.Game {
	[RequireComponent(typeof(Text))]
	public class BalanceText : MonoBehaviour {
		IEvent _event;
		Text   _text;

		[Inject]
		public void Init(IEvent events) {
			_event = events;
		}

		void OnEnable() {
			_event?.Subscribe<Balance_Changed>(this, OnBalanceChanged);
		}

		void OnDisable() {
			_event?.Unsubscribe<Balance_Changed>(OnBalanceChanged);
		}

		void Start() {
			_text = GetComponent<Text>();
		}

		void OnBalanceChanged(Balance_Changed e) {
			if ( _text ) {
				_text.text = e.NewBalance.ToString();
			}
		}
	}
}
