using UnityEngine;
using UDBase.UI.Common;
using UDBase.Controllers.EventSystem;
using Serverfull.Models;
using Serverfull.Events;
using Zenject;

namespace Serverfull.UI.Common {
	public class MessageManager : MonoBehaviour {
		public MessageView DialogPrefab;
		public MessageView OverlayPrefab;

		IEvent    _event;
		UIManager _ui;

		[Inject]
		public void Init(IEvent events, UIManager ui) {
			_event = events;
			_ui    = ui;
		}

		void OnEnable() {
			_event?.Subscribe<Message_New>(this, OnNewMessage);
		}

		void OnDisable() {
			_event?.Unsubscribe<Message_New>(OnNewMessage);
		}

		void OnNewMessage(Message_New e) {
			if ( e.OnNegative != null ) {
				_ui.ShowDialog(DialogPrefab.gameObject, () => e.OnPositive.Invoke(), () => e.OnNegative.Invoke(), go => InitDialog(go, e.Message));
			} else {
				_ui.ShowOverlay(OverlayPrefab.gameObject, () => e.OnPositive?.Invoke(), go => InitDialog(go, e.Message));
			}
		}

		void InitDialog(GameObject go, Message msg) {
			var view = go.GetComponent<MessageView>();
			view.TitleText.text   = msg.Title;
			view.ContentText.text = msg.Content;
		}
	}
}
