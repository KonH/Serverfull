using Serverfull.Views;
using Serverfull.Events;
using UDBase.Controllers.EventSystem;
using UnityEngine;
using Zenject;

namespace Serverfull.Game {
	[RequireComponent(typeof(Camera))]
	public class CommonRaycaster : MonoBehaviour {
		public float MaxDistance = 10000;

		Camera _camera;
		IEvent _event;

		[Inject]
		public void Init(IEvent events) {
			_event = events;
		}

		void Start() {
			_camera = GetComponent<Camera>();
		}

		void Update() {
			var ray = _camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit info;
			if ( Physics.Raycast(ray, out info, MaxDistance) ) {
				var go = info.collider?.gameObject;
				if ( go ) {
					var isClick = Input.GetMouseButton(0);
					var server  = go.GetComponent<ServerView>();
					if ( isClick ) {
						if ( server ) {
							_event.Fire(new UI_ServerSelected(server.Id));
							return;
						}
						_event.Fire(new UI_NothingSelected());
					}
				}
			}
		}
	}
}