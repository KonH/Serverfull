using System.Collections.Generic;
using UnityEngine;
using Serverfull.Controllers;
using Zenject;
using Serverfull.Models;

namespace Serverfull.UI.Game {
	public class EngineersPanel : MonoBehaviour {
		public EngineerView  Prefab;
		public RectTransform ContentRoot;

		List<EngineerView> _views = new List<EngineerView>();

		EngineerController _engineer;

		[Inject]
		public void Init(EngineerController engineer) {
			_engineer = engineer;
		}

		void Update() {
			foreach ( var view in _views ) {
				view.UpdateCanHire(_engineer.CanHire(view.Id));
			}
			var engineerIds = _engineer.Available;
			if ( NeedToUpdate(engineerIds) ) {
				Hide();
				var fullClients = _engineer.Get(engineerIds);
				Show(fullClients);
			}
		}

		void Show(List<Engineer> units) {
			foreach ( var unit in units ) {
				var view = ObjectPool.Spawn(Prefab, ContentRoot);
				view.Init(unit, _engineer.CanHire(unit), OnHire);
				_views.Add(view);
			}
		}

		public void Hide() {
			foreach ( var view in _views ) {
				ObjectPool.Recycle(view);
			}
			_views.Clear();
		}

		public bool NeedToUpdate(List<EngineerId> units) {
			if ( units.Count != _views.Count ) {
				return true;
			}
			foreach ( var view in _views ) {
				if ( !units.Contains(view.Id) ) {
					return true;
				}
			}
			return false;
		}

		void OnHire(EngineerId id) {
			_engineer.Hire(id);
		}
	}
}