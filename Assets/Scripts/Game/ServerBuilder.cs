﻿using UnityEngine;
using Serverfull.Models;
using Serverfull.Controllers;
using Zenject;

namespace Serverfull.Game {
	public class ServerBuilder : MonoBehaviour {
		public ServerPlaceholder PlaceholderPrefab;
		public Camera            RaycastCam;
		public string            RaycastLayer;

		UpgradeController     _upgrade;
		FinanceController     _finance;
		ServerBuildController _serverBuild;
		int                   _raycastMask;

		bool              _inBuildProcess;
		ServerPlaceholder _curPlaceholder;

		[Inject]
		public void Init(UpgradeController upgrade, FinanceController finance, ServerBuildController serverBuild) {
			_upgrade     = upgrade;
			_finance     = finance;
			_serverBuild = serverBuild;

			_raycastMask = LayerMask.GetMask(RaycastLayer);
		}

		public Money GetBuildPrice(ServerType type) {
			return _upgrade.GetUpgradeLevelInfo(type, 0).Price;
		}
		public Money GetMaintenance(ServerType type) {
			return _upgrade.GetUpgradeLevelInfo(type, 0).Maintenance;
		}
		public bool CanStartPlacement(ServerType type) {
			return (_finance.Balance >= GetBuildPrice(type)) && !_inBuildProcess;
		}
		public bool CanEndPlacement(ServerType type) {
			return _finance.Balance >= GetBuildPrice(type);
		}

		public void StartPlacement(ServerType type) {
			_inBuildProcess = true;
			if ( _curPlaceholder ) {
				_curPlaceholder.Show(type);
			} else {
				_curPlaceholder = Instantiate(PlaceholderPrefab);
			}
		}

		public void StopPlacement() {
			_inBuildProcess = false;
			_curPlaceholder.Hide();
		}

		void Update() {
			if ( !_inBuildProcess ) {
				return;
			}
			var pos = GetDesiredPosition();
			var isValid = CanEndPlacement(_curPlaceholder.Type) && _serverBuild.IsValidPosition(pos.x, pos.y);
			_curPlaceholder.UpdateState(pos, isValid);
			if ( Input.GetMouseButtonDown(0) ) {
				if ( isValid ) {
					_serverBuild.AddServer(_curPlaceholder.Type, pos.x, pos.y);
				}
				StopPlacement();
			}
		}

		Vector2Int GetDesiredPosition() {
			var ray = RaycastCam.ScreenPointToRay(Input.mousePosition);
			RaycastHit info;
			if ( Physics.Raycast(ray, out info, 1000, _raycastMask) ) {
				var p = info.point;
				return new Vector2Int(Mathf.RoundToInt(p.x), Mathf.RoundToInt(p.z));
			}
			return Vector2Int.zero;
		}
	}
}
