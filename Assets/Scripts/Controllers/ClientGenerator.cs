using System;
using System.Collections.Generic;
using System.Linq;
using UDBase.Utils;
using Serverfull.Common;
using Serverfull.Models;

namespace Serverfull.Controllers {
	public class ClientGenerator {
		readonly GameSettings _settings;

		List<ClientSettings> _clients        = new List<ClientSettings>();
		List<float>          _weights        = new List<float>();
		List<string>         _availableNames = new List<string>();
		List<ClientId>       _usedIds        = new List<ClientId>();

		public ClientGenerator(GameSettings settings) {
			_settings = settings;
		}

		ClientSettings SelectSetup() {
			_clients.Clear();
			_clients.AddRange(_settings.ClientSetups);
			_weights.Clear();
			foreach ( var client in _clients ) {
				_weights.Add(client.Chance);
			}
			return RandomUtils.GetItem(_clients, _weights);
		}

		ClientId GenerateId() {
			_availableNames.Clear();
			_availableNames.AddRange(_settings.ClientNames);
			foreach ( var id in _usedIds ) {
				_availableNames.Remove(id.Name);
			}
			if ( _availableNames.Count == 0 ) {
				return ClientId.Empty;
			}
			var newId = new ClientId(RandomUtils.GetItem(_availableNames));
			_usedIds.Add(newId);
			return newId;
		}

		List<ServerType> SelectServerTypes(int count) {
			var items = new List<ServerType>(Enum.GetValues(typeof(ServerType)) as ServerType[]);
			items.Remove(ServerType.Client);
			while ( items.Count > count ) {
				var randItem = RandomUtils.GetItem(items);
				items.Remove(randItem);
			}
			return items;
		}
		
		public Client CreateClient() {
			var id = GenerateId();
			if ( id.IsEmpty ) {
				return null;
			}
			var setup = SelectSetup();
			var client = new Client(
				id,
				new Money(setup.Income.RandomInclusive()),
				setup.Difficulty,
				setup.UserRate.RandomInclusive(),
				setup.Network.RandomInclusive(),
				setup.CPU.RandomInclusive(),
				setup.RAM.RandomInclusive(),
				SelectServerTypes(setup.PlusServers.RandomInclusive())
			);
			return client;
		}
	}
}
