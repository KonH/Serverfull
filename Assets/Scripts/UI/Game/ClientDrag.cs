using UnityEngine;
using UnityEngine.EventSystems;

namespace Serverfull.UI.Game {
	[RequireComponent(typeof(ClientView))]
	public class ClientDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
		ClientView _view;

		void Start() {
			_view = GetComponent<ClientView>();
		}

		public void OnBeginDrag(PointerEventData eventData) {
			var canvas = GetComponentInParent<Canvas>();
			transform.SetParent(canvas.transform);
		}

		public void OnDrag(PointerEventData eventData) {
			transform.position = eventData.position;
		}

		public void OnEndDrag(PointerEventData eventData) {
			foreach ( var panel in ClientsPanel.Instances ) {
				if ( RectTransformUtility.RectangleContainsScreenPoint(panel.ContentRoot, eventData.position) && panel.AddClient(_view) ) {
					break;
				}
			}
			_view.Owner.FreeView(_view);
		}
	}
}
