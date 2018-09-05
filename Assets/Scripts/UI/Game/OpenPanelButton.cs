using Serverfull.Events;
using UDBase.Controllers.EventSystem;
using UDBase.UI.Common;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Game {
	[RequireComponent(typeof(Button))]
	public class OpenPanelButton : MonoBehaviour {
		public PanelType Type;
		public UIElement Element;

		IEvent _event;
		
		bool _tracking;
		
		[Inject]
		public void Init(IEvent events) {
			_event = events;
			GetComponent<Button>().onClick.AddListener(Switch);
		}
		
		void Switch() {
			if ( Element.State != UIElement.UIElementState.Shown ) {
				_tracking = true;
			}
			Element.Switch();
		}

		void Update() {
			if ( _tracking ) {
				if ( Element.State == UIElement.UIElementState.Shown ) {
					_event.Fire(new Panel_Open(Type));
					_tracking = false;
				}
			}
		}
	}
}