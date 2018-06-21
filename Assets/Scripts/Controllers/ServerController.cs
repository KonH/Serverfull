using System.Collections.Generic;
using UDBase.Utils;
using UDBase.Controllers.LogSystem;
using Serverfull.Models;
using Serverfull.Game;

namespace Serverfull.Controllers {
	public class ServerController : ILogContext {
		List<Server> _servers = new List<Server>();

		readonly ULogger _log;

		public ServerController(ILog log, GameSettings settings) {
			_log = log.CreateLogger(this);
			var resources = new Dictionary<string, int> {
				{ Server.Network, settings.ServerNetwork },
				{ Server.RAM,     settings.ServerRAM     },
				{ Server.CPU,     settings.ServerCPU     }
			};
			for ( var i = 0; i < 3; i++ ) {
				_servers.Add(new Server(ServerId.Create(), new Money(settings.ServerMaintenance), settings.NetworkTime, settings.ProcessTime, resources));
			}
		}

		public Server GetServerForRequest(Request request) {
			return RandomUtils.GetItem(_servers);
		}

		public bool TryLockResource(Server server, string key, int value) {
			int freeValue;
			if ( server.Resources.TryGetValue(key, out freeValue) ) {
				if ( freeValue >= value ) {
					server.Resources[key] -= value;
					_log.MessageFormat("TryLockResource: {0}", server);
					return true;
				}
			}
			return false;
		}

		public void ReleaseResource(Server server, string key, int value) {
			if ( server.Resources.ContainsKey(key) ) {
				server.Resources[key] += value;
				_log.MessageFormat("ReleaseResource: {0}", server);
			}
		}

		public Money GetTotalMaintenance() {
			var result = Money.Zero;
			foreach ( var server in _servers ) {
				result += server.Maintenance;
			}
			return result;
		}
	}
}
