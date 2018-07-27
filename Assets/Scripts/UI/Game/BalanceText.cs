using UnityEngine;
using UnityEngine.UI;
using UDBase.Controllers.EventSystem;
using Serverfull.Events;
using Serverfull.Models;
using Serverfull.Controllers;
using Zenject;

namespace Serverfull.UI.Game {
	[RequireComponent(typeof(Text))]
	public class BalanceText : MonoBehaviour {
		IEvent            _event;
		FinanceController _finance;

		Text   _text;

		[Inject]
		public void Init(IEvent events, FinanceController finance) {
			_event   = events;
			_finance = finance;
		}

		void OnEnable() {
			_event?.Subscribe<Balance_Changed>(this, OnBalanceChanged);
		}

		void OnDisable() {
			_event?.Unsubscribe<Balance_Changed>(OnBalanceChanged);
		}

		void Start() {
			_text = GetComponent<Text>();
			UpdateBalance(_finance.Balance);
		}

		void OnBalanceChanged(Balance_Changed e) {
			UpdateBalance(e.NewBalance);
		}

		void UpdateBalance(Money value) {
			if ( _text ) {
				_text.text = value.ToString();
			}
		}
	}
}
