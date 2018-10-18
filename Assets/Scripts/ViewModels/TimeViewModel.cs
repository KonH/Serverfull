using System;
using System.ComponentModel;
using UnityEngine;
using Serverfull.Controllers;
using Zenject;
using UnityWeld.Binding;

namespace Serverfull.ViewModels {
	[Binding]
	public class TimeViewModel : MonoBehaviour, INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;

		[Binding]
		public DateTime Time {
			get {
				return _time.GameTime;
			}
		}

		TimeController _time;

		[Inject]
		public void Init(TimeController time) {
			_time = time;
		}

		void OnEnable() {
			_time.State.PropertyChanged += OnPropertyChanged;
		}

		void OnDisable() {
			_time.State.PropertyChanged -= OnPropertyChanged;
		}

		void OnPropertyChanged(object sender, PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);
	}
}
