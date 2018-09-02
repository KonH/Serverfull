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
		static List<EngineerId> _tempIds   = new List<EngineerId>();
		static List<Engineer>   _tempUnits = new List<Engineer>();

		public List<EngineerId> Available => GetUnitsByHired(false);
		public List<EngineerId> Hired     => GetUnitsByHired(true);

		readonly IEvent            _event;
		readonly FinanceController _finance;

		Dictionary<EngineerId, Engineer> _units = new Dictionary<EngineerId, Engineer>();

		public EngineerController(IEvent events, GameSettings settings, FinanceController finance) {
			_event   = events;
			_finance = finance;
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

		public void AddUnit(Engineer unit) {
			_units.Add(unit.Id, unit);
			_event.Fire(new Engineer_New(unit.Id));
		}

		public Engineer Get(EngineerId id) => DictUtils.GetOrDefault(_units, id);

		public List<Engineer> Get(List<EngineerId> ids) {
			_tempUnits.Clear();
			var result = _tempUnits;
			foreach ( var id in ids ) {
				var unit = Get(id);
				if ( unit != null ) {
					result.Add(unit);
				}
			}
			return result;
		}

		public List<EngineerId> GetUnitsByHired(bool value) {
			_tempIds.Clear();
			var result = _tempIds;
			foreach ( var unit in _units ) {
				if ( unit.Value.Hired == value ) {
					result.Add(unit.Key);
				}
			}
			return result;
		}

		public bool CanHire(Engineer unit) {
			return (unit != null) && (_finance.Balance >= unit.Price);
		}

		public bool CanHire(EngineerId id) {
			var unit = Get(id);
			return CanHire(unit);
		}

		public void Hire(EngineerId id) {
			var unit = Get(id);
			if ( CanHire(unit) ) {
				_finance.Spend(unit.Price);
				unit.Hire();
				_event.Fire(new Engineer_Hired(unit.Id));
			}
		}
	}
}
