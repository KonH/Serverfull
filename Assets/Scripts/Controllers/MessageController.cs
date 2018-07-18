using System;
using UDBase.Controllers.LogSystem;
using UDBase.Controllers.EventSystem;
using UDBase.Controllers.SceneSystem;
using Serverfull.Events;
using Serverfull.Models;
using Zenject;

namespace Serverfull.Controllers {
	public class MessageController : ILogContext, IInitializable, IDisposable {
		readonly IEvent  _event;
		readonly ULogger _logger;
		readonly IScene  _scene;

		public MessageController(IEvent events, ILog log, IScene scene) {
			_event  = events;
			_logger = log.CreateLogger(this);
			_scene  = scene;
		}

		public void Initialize() {
			_event.Subscribe<Status_GameEnd>(this, OnGameEnd);
		}

		public void Dispose() {
			_event.Unsubscribe<Status_GameEnd>(OnGameEnd);
		}

		void OnGameEnd(Status_GameEnd e) {
			RaiseMessage("Game ended", "You have no money!", Restart);
		}


		void RaiseMessage(string title, string content, Action onPositive = null, Action onNegative = null) {
			_logger.MessageFormat("New message: {0} ({1})", title, content);
			_event.Fire(new Message_New(new Message(title, content), onPositive, onNegative));
		}

		void Restart() {
			_scene.ReloadScene();
		}
	}
}
