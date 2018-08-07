using System;
using System.Collections.Generic;
using UDBase.Utils;
using UDBase.Controllers.LogSystem;
using UDBase.Controllers.EventSystem;
using Serverfull.Models;
using Serverfull.Events;
using Zenject;

namespace Serverfull.Controllers {
	public class ServerController : ILogContext, IInitializable, IDisposable {
		public IEnumerable<Server> All => _servers.Values;

		Dictionary<ServerId, Server> _servers = new Dictionary<ServerId, Server>();

		readonly ULogger           _log;
		readonly IEvent            _event;
		readonly FinanceController _finance;

		public ServerController(ILog log, IEvent events, FinanceController finance) {
			_log     = log.CreateLogger(this);
			_event   = events;
			_finance = finance;
		}

		public void Initialize() {
			_event.Subscribe<Time_NewGameHour>(this, OnNewHour);
		}

		public void Dispose() {
			_event.Unsubscribe<Time_NewGameHour>(OnNewHour);
		}

		void OnNewHour(Time_NewGameHour e) {
			_finance.Spend(GetTotalMaintenance());
		}

		public void Add(Server server) {
			_servers.Add(server.Id, server);
			_event.Fire(new Server_New(server.Id, server.PosX, server.PosY));
		}

		public Server Get(ServerId id) => _servers.GetOrDefault(id);

		public Server GetServerForRequest(Request request) {
			return GetClientServer(request.Owner.Client);
		}

		public void AddClientToServer(ClientId clientId, ServerId serverId) {
			var server = Get(serverId);
			_log.MessageFormat("AddClientToServer: {0} => {1} ({2})", clientId, serverId, server);
			server?.Clients.Add(clientId);
		}

		public void RemoveClientFromServer(ClientId clientId, ServerId serverId) {
			var server = Get(serverId);
			_log.MessageFormat("RemoveClientFromServer: {0} => {1} ({2})", clientId, serverId, server);
			server?.Clients.Remove(clientId);
		}

		public Server GetClientServer(ClientId clientId) {
			foreach ( var server in _servers.Values ) {
				if ( server.Clients.Contains(clientId) ) {
					return server;
				}
			}
			return null;
		}

		public bool TryLockResource(Server server, Server.Resource res, int value) {
			if ( res.Free >= value ) {
				res.Free -= value;
				_log.MessageFormat("TryLockResource: {0}", server);
				return true;
			}
			return false;
		}

		public void ReleaseResource(Server server, Server.Resource res, int value) {
			res.Free += value;
			_log.MessageFormat("ReleaseResource: {0}", server);
		}

		Money GetTotalMaintenance() {
			var result = Money.Zero;
			foreach ( var server in _servers.Values ) {
				result += server.Maintenance;
			}
			return result;
		}
	}
}
