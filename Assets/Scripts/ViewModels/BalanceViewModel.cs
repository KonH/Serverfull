using System.ComponentModel;
using UnityEngine;
using Serverfull.Models;
using Serverfull.Controllers;
using Zenject;
using UnityWeld.Binding;

namespace Serverfull.ViewModels {
	[Binding]
	public class BalanceViewModel : MonoBehaviour, INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;

		[Binding]
		public Money Balance {
			get {
				return _finance.Balance;
			}
		}

		FinanceController _finance;

		[Inject]
		public void Init(FinanceController finance) {
			_finance = finance;
		}

		void OnEnable() {
			_finance.State.PropertyChanged += OnPropertyChanged;
		}

		void OnDisable() {
			_finance.State.PropertyChanged -= OnPropertyChanged;
		}

		void OnPropertyChanged(object sender, PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);
	}
}
