using System.Collections.Generic;
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
			_clients.AddRange(_settings.Setups);
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
				setup.RAM.RandomInclusive()
				);
			return client;
		}
	}
}
