using System;
using System.ComponentModel;
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
		readonly TimeController    _time;
		readonly FinanceController _finance;

		public ServerController(ILog log, IEvent events, TimeController time, FinanceController finance) {
			_log     = log.CreateLogger(this);
			_event   = events;
			_time    = time;
			_finance = finance;
		}

		public void Initialize() {
			_time.State.PropertyChanged += OnTimeChanged;
		}

		public void Dispose() {
			_time.State.PropertyChanged -= OnTimeChanged;
		}

		void OnTimeChanged(object sender, PropertyChangedEventArgs e) {
			if ( e.PropertyName == nameof(TimeModel.Hour) ) {
				_finance.Spend(GetTotalMaintenance());
			}
		}

		public void Add(Server server) {
			_servers.Add(server.Id, server);
			_event.Fire(new Server_New(server.Id, server.PosX, server.PosY));
		}

		public Server Get(ServerId id) => _servers.GetOrDefault(id);

		public Server GetServerForRequest(Request request) {
			return
				(request.Type == ServerType.Client) ? GetClientServer(request.Owner.Client) : GetBestServerByType(request.Type);
		}

		public bool TryAddClientToServer(ClientId clientId, ServerId serverId) {
			var server = Get(serverId);
			if ( server == null ) {
				_log.ErrorFormat("AddClientToServer: can't find server by {0}", serverId);
				return false;
			}
			if ( server.Type != ServerType.Client ) {
				return false;
			}
			_log.MessageFormat("AddClientToServer: {0} => {1} ({2})", clientId, serverId, server);
			server.Clients.Add(clientId);
			return true;
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

		Server GetBestServerByType(ServerType type) {
			var freeNetwork = int.MinValue;
			Server minServer = null;
			foreach ( var server in _servers.Values ) {
				if ( (server.Type != type) || (server.Network.Free < freeNetwork) ) {
					continue;
				}
				freeNetwork = server.Network.Free;
				minServer = server;
			}
			return minServer;
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
