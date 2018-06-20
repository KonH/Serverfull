using System.Collections.Generic;
using UDBase.Utils;
using UDBase.Controllers.LogSystem;
using Serverfull.Models;
using Serverfull.Game;

namespace Serverfull.Controllers {
	public class ServerController : ILogContext {
		public List<Server> Servers { get; }

		readonly ULogger _log;

		public ServerController(ILog log, GameSettings settings) {
			_log = log.CreateLogger(this);
			var resources = new Dictionary<string, int> {
				{ Server.Network, settings.ServerNetwork },
				{ Server.RAM,     settings.ServerRAM     },
				{ Server.CPU,     settings.ServerCPU     }
			};
			Servers = new List<Server>();
			for ( var i = 0; i < 3; i++ ) {
				Servers.Add(new Server(ServerId.Create(), new Money(settings.ServerMaintenance), settings.NetworkTime, settings.ProcessTime, resources));
			}
		}

		public Server GetServerForRequest(Request request) {
			return RandomUtils.GetItem(Servers);
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
	}
}
