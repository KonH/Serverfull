using System;
using System.Collections.Generic;
using UDBase.Utils;
using UDBase.Controllers.EventSystem;
using Serverfull.Common;
using Serverfull.Models;
using Serverfull.Events;
using Zenject;

namespace Serverfull.Controllers {
	public class EngineerController : IInitializable, IDisposable {
		static List<EngineerId> _tempUnits = new List<EngineerId>();

		public IEnumerable<EngineerId> All       => _units.Keys;
		public IEnumerable<EngineerId> Available => GetUnitsByHired(false);
		public IEnumerable<EngineerId> Hired     => GetUnitsByHired(true);

		readonly IEvent            _event;
		readonly FinanceController _finance;

		Dictionary<EngineerId, Engineer> _units = new Dictionary<EngineerId, Engineer>();

		public EngineerController(IEvent events, GameSettings settings, FinanceController finance) {
			_event   = events;
			_finance = finance;
			AddUnit(new Engineer(new EngineerId("TestEngineer1"), settings.FixTime, new Money(5), new Money(5), false));
			AddUnit(new Engineer(new EngineerId("TestEngineer2"), settings.FixTime, new Money(5), new Money(5), false));
			AddUnit(new Engineer(new EngineerId("TestEngineer3"), settings.FixTime, new Money(5), new Money(5), false));
		}

		public void Initialize() {
			_event.Subscribe<Time_NewGameHour>(this, OnNewHour);
		}

		public void Dispose() {
			_event.Unsubscribe<Time_NewGameHour>(OnNewHour);
		}

		void OnNewHour(Time_NewGameHour e) {
			var salary = Money.Zero;
			foreach ( var id in Hired ) {
				salary += Get(id).Salary;
			}
			_finance.Spend(salary);
		}

		void AddUnit(Engineer unit) {
			_units.Add(unit.Id, unit);
		}

		public Engineer Get(EngineerId id) => DictUtils.GetOrDefault(_units, id);

		public IEnumerable<EngineerId> GetUnitsByHired(bool value) {
			_tempUnits.Clear();
			foreach ( var unit in _units ) {
				if ( unit.Value.Hired == value ) {
					_tempUnits.Add(unit.Key);
				}
			}
			return _tempUnits;
		}

		public void Hire(EngineerId id) {
			var unit = Get(id);
			if ( unit != null ) {
				if ( !unit.Hired && (_finance.Balance >= unit.Price) ) {
					_finance.Spend(unit.Price);
					unit.Hire();
					_event.Fire(new Engineer_New(unit.Id));
				}
			}
		}
	}
}
