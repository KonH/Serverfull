using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.EventSystem;
using Serverfull.Common;
using Serverfull.Models;
using Serverfull.Events;
using Zenject;

namespace Serverfull.Controllers {
	public class BreakController : ITickable {
		class BreakHolder {
			public int Network;
			public int CPU;
			public int RAM;

			public BreakHolder(int network, int cpu, int ram) {
				Network = network;
				CPU     = cpu;
				RAM     = ram;
			}
		}

		readonly GameRules        _rules;
		readonly ServerController _server;
		readonly TimeController   _time;
		readonly IEvent           _event;

		public HashSet<ServerId> BreakedServers { get; } = new HashSet<ServerId>();

		public BreakController(GameRules rules, ServerController server, TimeController time, IEvent events) {
			_rules  = rules;
			_server = server;
			_time   = time;
			_event  = events;
		}

		public void Tick() {
			var deltaTime = _time.DeltaTime;
			var chance = _rules.GetBreakChance(deltaTime);
			foreach ( var server in _server.All ) {
				if ( IsServerBreaked(server.Id) ) {
					continue;
				}
				if ( Random.value < chance ) {
					BreakServer(server);
				}
			}
		}

		public bool IsServerBreaked(ServerId id) {
			return BreakedServers.Contains(id);
		}

		void BreakServer(Server server) {
			var network = server.Network.Free;
			var cpu     = server.CPU.Free;
			var ram     = server.RAM.Free;
			if (
				_server.TryLockResource(server, server.Network, network) &&
				_server.TryLockResource(server, server.CPU, cpu) &&
				_server.TryLockResource(server, server.RAM, ram) ) {
				BreakedServers.Add(server.Id);
				_event.Fire(new Server_Break(server.Id));
			}
		}

		public void FixServer(ServerId id) {
			if ( BreakedServers.Remove(id) ) {
				var server = _server.Get(id);
				if ( server != null ) {
					_server.ReleaseResource(server, server.Network, server.Network.Max);
					_server.ReleaseResource(server, server.CPU, server.CPU.Max);
					_server.ReleaseResource(server, server.RAM, server.RAM.Max);
					_event.Fire(new Server_Fix(server.Id));
				}
			}
		}
	}
}
