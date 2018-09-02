using System.Collections.Generic;
using UDBase.Utils;
using Serverfull.Common;
using Serverfull.Models;

namespace Serverfull.Controllers {
	public class EngineerGenerator {
		readonly GameSettings _settings;

		List<EngineerSettings> _engineers      = new List<EngineerSettings>();
		List<float>            _weights        = new List<float>();
		List<string>           _availableNames = new List<string>();
		List<EngineerId>       _usedIds        = new List<EngineerId>();

		public EngineerGenerator(GameSettings settings) {
			_settings = settings;
		}

		EngineerSettings SelectSetup() {
			_engineers.Clear();
			_engineers.AddRange(_settings.EngineerSetups);
			_weights.Clear();
			foreach ( var client in _engineers ) {
				_weights.Add(client.Chance);
			}
			return RandomUtils.GetItem(_engineers, _weights);
		}

		EngineerId GenerateId() {
			_availableNames.Clear();
			_availableNames.AddRange(_settings.EngineerNames);
			foreach ( var id in _usedIds ) {
				_availableNames.Remove(id.Name);
			}
			if ( _availableNames.Count == 0 ) {
				return EngineerId.Empty;
			}
			var newId = new EngineerId(RandomUtils.GetItem(_availableNames));
			_usedIds.Add(newId);
			return newId;
		}

		public Engineer CreateEngineer() {
			var id = GenerateId();
			if ( id.IsEmpty ) {
				return null;
			}
			var setup = SelectSetup();
			var engineer = new Engineer(
				id,
				setup.FixTime.Random(),
				new Money(setup.Price.RandomInclusive()),
				new Money(setup.Salary.RandomInclusive()),
				setup.Level,
				false
			);
			return engineer;
		}
	}
}
