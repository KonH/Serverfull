using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UDBase.Controllers.EventSystem;
using Serverfull.Events;
using Zenject;
using DG.Tweening;

namespace Serverfull.UI.Common {
	public class NotificationManager : MonoBehaviour {
		public Text NotificationText;

		IEvent _event;

		Queue<string> _notifications = new Queue<string>();
		Sequence      _seq           = null;

		[Inject]
		public void Init(IEvent events) {
			_event = events;
		}

		void Awake() {
			NotificationText.text = string.Empty;
		}

		void Start() {
			_event.Subscribe<Notification_New>(this, OnNewNotification);
		}

		void OnDestroy() {
			_event.Unsubscribe<Notification_New>(OnNewNotification);
		}

		void OnNewNotification(Notification_New e) {
			_notifications.Enqueue(e.Message);			
		}

		void Update() {
			if ( _seq != null ) {
				return;
			}
			if ( _notifications.Count > 0 ) {
				var msg = _notifications.Dequeue();
				NotificationText.text = msg;
				var trans = NotificationText.transform;
				trans.localScale = Vector3.zero;
				_seq = DOTween.Sequence();
				_seq.Append(trans.DOScale(Vector3.one, 0.50f).SetEase(Ease.OutBounce));
				_seq.AppendInterval(1.5f);
				_seq.Append(trans.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBounce));
				_seq.AppendInterval(0.25f);
				_seq.OnComplete(() => _seq = null);
			}
		}
	}
}
