using System;
using UDBase.Controllers.EventSystem;
using UDBase.Controllers.LogSystem;
using Serverfull.Game;
using Serverfull.Events;
using Serverfull.Models;
using Zenject;

namespace Serverfull.Controllers {
	class ClientMoodController : ILogContext, IInitializable, IDisposable {
		readonly ULogger           _logger;
		readonly IEvent            _event;
		readonly GameRules         _rules;
		readonly ClientController  _client;
		readonly RequestController _request;

		public ClientMoodController(ILog log, IEvent events, GameRules rules, ClientController client, RequestController request) {
			_logger  = log.CreateLogger(this);
			_event   = events;
			_rules   = rules;
			_client  = client;
			_request = request;
		}
		
		public void Initialize() {
			_event.Subscribe<Request_NewStatus>(this, OnRequestNewStatus);
		}

		public void Dispose() {
			_event.Unsubscribe<Request_NewStatus>(OnRequestNewStatus);
		}

		void OnRequestNewStatus(Request_NewStatus e) {
			if ( e.NewStatus == RequestStatus.Finished ) {
				var req    = _request.Get(e.Id);
				var user   = req.Owner;
				var client = user.Client;
				var change = _rules.CalculateClientMoodChange(user.Mood);
				_logger.MessageFormat("Update client {0} mood to: {1} (from user mood: {2})", client, change, user.Mood);
				_client.UpdateMood(client, change);
				var newMood = _client.Get(client)?.Mood;
				_logger.MessageFormat("New client {0} mood is {1}", client, newMood);
				if ( newMood <= 0 ) {
					_client.RemoveClient(client);
				}
			}
		}
	}
}
