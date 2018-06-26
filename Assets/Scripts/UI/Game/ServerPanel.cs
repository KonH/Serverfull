using UnityEngine;
using UnityEngine.UI;
using UDBase.Controllers.EventSystem;
using Serverfull.Events;
using Serverfull.Models;
using Serverfull.Controllers;
using Zenject;

namespace Serverfull.UI.Game {
	public class ServerPanel : MonoBehaviour {
		const float UpdateTime = 0.33f;
		

		public GameObject         Root;
		public Slider             NetworkSlider;
		public Slider             CpuSlider;
		public Slider             RamSlider;
		public ServerClientsPanel Clients;
		public Button             UpgradeButton;
		public Text               UpgradeText;

		IEvent            _event;
		ServerController  _server;
		UpgradeController _upgrade;
		float             _timer;
		ServerId          _selectedId;
		int               _cachedUpgradeLevel = -1;

		void Awake() {
			UpgradeButton.onClick.AddListener(OnUpgradeClick);
		}

		[Inject]
		public void Init(IEvent events, ServerController server, UpgradeController upgrade) {
			_event   = events;
			_server  = server;
			_upgrade = upgrade;
		}

		void OnEnable() {
			_event?.Subscribe<UI_ServerSelected>(this, OnServerSelected);
		}

		void OnDisable() {
			_event?.Unsubscribe<UI_ServerSelected>(OnServerSelected);
		}

		void OnServerSelected(UI_ServerSelected e) {
			_selectedId      = e.Id;
			Clients.ServerId = e.Id;
			_cachedUpgradeLevel    = -1;
		}

		void Start() {
			UpdateState();
		}

		void Update() {
			UpdateState();
		}

		void UpdateState() {
			var server = _server.Get(_selectedId);
			Root.SetActive(server != null);
			if ( server != null ) {
				_timer += Time.deltaTime;
				if ( _timer < UpdateTime ) {
					return;
				}
				_timer = 0.0f;
				UpdateResource(server.Network, NetworkSlider);
				UpdateResource(server.CPU,     CpuSlider);
				UpdateResource(server.RAM,     RamSlider);
				UpdateUpgradeButton(server);
			} else {
				_timer = UpdateTime;
			}
		}

		void UpdateResource(Server.Resource serverRes, Slider resSlider) {
			resSlider.value = serverRes.NormalizedFree;
		}

		void UpdateUpgradeButton(Server server) {
			UpgradeButton.interactable = _upgrade.CanUpgrade(server.Id);
			if ( server.UpgradeLevel == _cachedUpgradeLevel ) {
				return;
			}
			var upgradeInfo = _upgrade.GetUpgradeLevelInfo(server.UpgradeLevel + 1);
			UpgradeButton.gameObject.SetActive(upgradeInfo != null);
			if ( upgradeInfo != null ) {
				UpgradeText.text = string.Format("Upgrade ({0})", upgradeInfo.Price);
			}
			_cachedUpgradeLevel = server.UpgradeLevel;
		}

		void OnUpgradeClick() {
			_upgrade.Upgrade(_selectedId);
			_cachedUpgradeLevel = -1;
		}
	}
}
