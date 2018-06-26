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

		ServerController _server;
		IEvent           _event;
		float            _timer;
		ServerId         _selectedId;

		[Inject]
		public void Init(IEvent events, ServerController server) {
			_event  = events;
			_server = server;
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

			} else {
				_timer = UpdateTime;
			}
		}

		void UpdateResource(Server.Resource serverRes, Slider resSlider) {
			resSlider.value = serverRes.NormalizedFree;
		}
	}
}
