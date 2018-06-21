using System.Collections.Generic;
using UDBase.Utils;
using UDBase.Controllers.LogSystem;
using Serverfull.Models;
using Serverfull.Game;

namespace Serverfull.Controllers {
	public class ServerController : ILogContext {
		Dictionary<ServerId, Server> _servers = new Dictionary<ServerId, Server>();

		readonly ULogger _log;

		public ServerController(ILog log, GameSettings settings) {
			_log = log.CreateLogger(this);
			var resources = new Dictionary<string, int> {
				{ Server.Network, settings.ServerNetwork },
				{ Server.RAM,     settings.ServerRAM     },
				{ Server.CPU,     settings.ServerCPU     }
			};
			for ( var i = 0; i < 3; i++ ) {
				var id = ServerId.Create();
				_servers.Add(id, new Server(id, new Money(settings.ServerMaintenance), settings.NetworkTime, settings.ProcessTime, resources));
			}
		}

		public Server Get(ServerId id) => _servers.GetOrDefault(id);

		public Server GetServerForRequest(Request request) {
			return GetClientServer(request.Owner.Client);
		}

		public void AddClientToServer(ClientId clientId, ServerId serverId) {
			var server = Get(serverId);
			server?.Clients.Add(clientId);
		}

		public Server GetClientServer(ClientId clientId) {
			foreach ( var server in _servers.Values ) {
				if ( server.Clients.Contains(clientId) ) {
					return server;
				}
			}
			return null;
		}

		public bool TryLockResource(Server server, string key, int value) {
			Server.Resource res;
			if ( server.Resources.TryGetValue(key, out res) ) {
				if ( res.Free >= value ) {
					res.Free -= value;
					_log.MessageFormat("TryLockResource: {0}", server);
					return true;
				}
			}
			return false;
		}

		public void ReleaseResource(Server server, string key, int value) {
			if ( server.Resources.ContainsKey(key) ) {
				server.Resources[key].Free += value;
				_log.MessageFormat("ReleaseResource: {0}", server);
			}
		}

		public Money GetTotalMaintenance() {
			var result = Money.Zero;
			foreach ( var server in _servers.Values ) {
				result += server.Maintenance;
			}
			return result;
		}
	}
}
