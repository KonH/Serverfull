using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UDBase.Utils;
using UDBase.Controllers.EventSystem;
using Serverfull.Events;
using Serverfull.Models;
using Serverfull.Controllers;
using Zenject;

namespace Serverfull.UI.Game {
	public class ServerPanel : MonoBehaviour {
		const float UpdateTime = 0.33f;
		
		[Serializable]
		public class ServerResourcePanel {
			public string Name;
			public Slider Slider;
		}

		public GameObject                Root;
		public List<ServerResourcePanel> Resources;
		public ClientsPanel              Clients;

		ServerController _server;
		ClientController _client;
		IEvent           _event;
		float            _timer;
		ServerId         _selectedId;

		[Inject]
		public void Init(IEvent events, ServerController server, ClientController client) {
			_event  = events;
			_server = server;
			_client = client;
		}

		void OnEnable() {
			_event?.Subscribe<UI_ServerSelected> (this, OnServerSelected);
			_event?.Subscribe<UI_NothingSelected>(this, OnNothingSelected);
		}

		void OnDisable() {
			_event?.Unsubscribe<UI_ServerSelected> (OnServerSelected);
			_event?.Unsubscribe<UI_NothingSelected>(OnNothingSelected);
		}

		void OnServerSelected(UI_ServerSelected e) {
			_selectedId = e.Id;
		}

		void OnNothingSelected(UI_NothingSelected e) {
			_selectedId = new ServerId(-1);
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
				foreach ( var res in Resources ) {
					var value = server.Resources.GetOrDefault(res.Name);
					res.Slider.value = value != null ? value.NormalizedFree : 0.0f;
				}
				if ( Clients.NeedToUpdate(server.Clients) ) {
					var fullClients = _client.Get(server.Clients);
					Clients.Hide();
					Clients.Show(fullClients);
				}

			} else {
				_timer = UpdateTime;
				Clients.Hide();
			}
		}
	}
}
