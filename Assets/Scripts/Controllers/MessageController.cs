using System;
using UDBase.Controllers.EventSystem;
using UDBase.Controllers.LogSystem;
using Serverfull.Events;
using Serverfull.Models;

namespace Serverfull.Controllers {
	public class MessageController : ILogContext {
		readonly IEvent  _event;
		readonly ULogger _logger;

		public MessageController(IEvent events, ILog log) {
			_event  = events;
			_logger = log.CreateLogger(this);
		}

		void RaiseMessage(string title, string content, Action onPositive = null, Action onNegative = null) {
			_logger.MessageFormat("New message: {0} ({1})", title, content);
			_event.Fire(new Message_New(new Message(title, content), onPositive, onNegative));
		}
	}
}
