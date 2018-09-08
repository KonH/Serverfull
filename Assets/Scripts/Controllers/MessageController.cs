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
			_event.Subscribe<Client_Lost>   (this, OnClientLost);
			_event.Subscribe<Server_Break>  (this, OnServerBreak);
		}

		public void Dispose() {
			_event.Unsubscribe<Status_GameEnd>(OnGameEnd);
			_event.Unsubscribe<Client_Lost>   (OnClientLost);
			_event.Unsubscribe<Server_Break>  (OnServerBreak);
		}

		void OnGameEnd(Status_GameEnd e) {
			RaiseMessage("Game ended", "You have no money!", Restart);
		}

		void OnClientLost(Client_Lost e) {
			RaiseNotification($"Client '{e.Id.Name}' break with us, our service sucks!");
		}

		void OnServerBreak(Server_Break e) {
			RaiseNotification("Our server is broken!");
		}

		void RaiseMessage(string title, string content, Action onPositive = null, Action onNegative = null) {
			_logger.MessageFormat("New message: {0} ({1})", title, content);
			_event.Fire(new Message_New(new Message(title, content), onPositive, onNegative));
		}

		void RaiseNotification(string message) {
			_logger.MessageFormat("New notification: {0}", message);
			_event.Fire(new Notification_New(message));
		}

		void Restart() {
			_scene.ReloadScene();
		}
	}
}
