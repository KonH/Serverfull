using System.Collections.Generic;
using UDBase.Controllers.LogSystem;

public class ServerController : ILogContext {
	ULogger _log;
	Server _instance;

	public ServerController(ILog log, GameSettings settings) {
		_log = log.CreateLogger(this);
		var resources = new Dictionary<string, int> {
			{ Server.Network, settings.ServerNetwork },
			{ Server.RAM, settings.ServerRAM },
			{ Server.CPU, settings.ServerCPU }
		};
		_instance = new Server(settings.NetworkTime, settings.ProcessTime, resources);
	}

	public Server GetServerForRequest(Request request) {
		return _instance;
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
