using UnityEngine;
using UDBase.Controllers.EventSystem;
using Serverfull.Events;
using Zenject;

namespace Serverfull.UI.Game {
	public class TutorialManager : MonoBehaviour {
		public GameObject BlockLayer;

		IEvent _event;

		TutorialItem _curTutorial;

		[Inject]
		public void Init(IEvent events) {
			_event = events;
		}

		void Start() {
			_event.Subscribe<Tutorial_New>(this, OnTutorialNew);
		}

		void OnDestroy() {
			_event.Unsubscribe<Tutorial_New>(OnTutorialNew);
		}

		void OnTutorialNew(Tutorial_New e) {
			var child = FindTutorialChild(e.Name);
			Debug.Assert(child);
			if ( child ) {
				_curTutorial = child;
				_curTutorial.gameObject.SetActive(true);
				BlockLayer.SetActive(child.WithBlockLayer);
			} else {
				CompleteTutorial(e.Name);
			}
		}

		TutorialItem FindTutorialChild(string tutName) {
			for ( var i = 0; i < transform.childCount; i++ ) {
				var child = transform.GetChild(i);
				if ( child.name == tutName ) {
					return child.gameObject.GetComponent<TutorialItem>();
				}
			}
			return null;
		}

		void CompleteTutorial(string tutName) {
			_event.Fire(new Tutorial_Complete(tutName));
		}

		void Update() {
			if ( Input.anyKeyDown ) {
				if ( _curTutorial ) {
					var tutName = _curTutorial.name;
					_curTutorial.gameObject.SetActive(false);
					BlockLayer.SetActive(false);
					_curTutorial = null;
					CompleteTutorial(tutName);
				}
			}
		}
	}
}
