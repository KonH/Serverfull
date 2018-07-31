using System.Collections.Generic;
using UDBase.Utils;
using Serverfull.Game;
using Serverfull.Models;

namespace Serverfull.Controllers {
	public class EngineerController {
		public IEnumerable<EngineerId> All => _units.Keys;

		Dictionary<EngineerId, Engineer> _units = new Dictionary<EngineerId, Engineer>();

		public EngineerController(GameSettings settings) {
			AddUnit(new Engineer(new EngineerId("TestEngineer"), settings.FixTime));
		}

		void AddUnit(Engineer unit) {
			_units.Add(unit.Id, unit);
		}

		public Engineer Get(EngineerId id) => DictUtils.GetOrDefault(_units, id);
	}
}
