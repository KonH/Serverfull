using UnityEngine;
using System.Collections.Generic;
using UDBase.Controllers.EventSystem;
using Serverfull.Views;
using Serverfull.Events;
using Serverfull.Models;
using Serverfull.Controllers;
using Zenject;

namespace Serverfull.Game {
	public class EngineerManager : MonoBehaviour {
		public Transform    SpawnPoint;
		public EngineerUnit EngineerPrefab;

		IEvent             _event;
		EngineerController _engineer;
		BreakController    _break;
		ServerManager      _server;

		Dictionary<EngineerId, EngineerUnit> _units             = new Dictionary<EngineerId, EngineerUnit>();
		HashSet<ServerId>                    _inProgressServers = new HashSet<ServerId>();

		[Inject]
		public void Init(IEvent events, EngineerController engineer, BreakController breaking, ServerManager server) {
			_event    = events;
			_engineer = engineer;
			_break    = breaking;
			_server   = server;
		}

		void OnEnable() {
			_event?.Subscribe<Engineer_New>(this, OnNewEngineer);
		}

		void OnDisable() {
			_event?.Unsubscribe<Engineer_New>(OnNewEngineer);
		}

		void Start() {
			// temp
			foreach ( var id in _engineer.Available ) {
				_engineer.Hire(id);
			}
		}

		void OnNewEngineer(Engineer_New e) {
			SpawnUnit(e.Id);
		}

		void SpawnAllUnits() {
			foreach ( var eng in _engineer.Hired ) {
				SpawnUnit(eng);
			}
		}

		void SpawnUnit(EngineerId id) {
			var instance = ObjectPool.Spawn(EngineerPrefab, SpawnPoint.position);
			instance.Init(this, id);
			_units.Add(id, instance);
		}

		EngineerUnit GetFreeEngineer() {
			foreach ( var unit in _units.Values ) {
				if ( !unit.IsBusy ) {
					return unit;
				}
			}
			return null;
		}

		void Update() {
			foreach ( var server in _break.BreakedServers ) {
				if ( _inProgressServers.Contains(server) ) {
					continue;
				}
				var view = _server.GetView(server);
				if ( view != null ) {
					var engineer = GetFreeEngineer();
					if ( engineer != null ) {
						engineer.GoToFixServer(view);
						_inProgressServers.Add(view.Id);
					}
				}
			}
		}

		public void DoneFixServer(EngineerId id, ServerView server) {
			_break.FixServer(server.Id);
			_inProgressServers.Remove(server.Id);
		}

		public float GetFixTime(EngineerId id) {
			return _engineer.Get(id).FixTime;
		}
	}
}
