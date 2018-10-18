using System.ComponentModel;
using UDBase.Controllers.SaveSystem;
using FullSerializer;

namespace Serverfull.Models {
	public class FinanceModel : INotifyPropertyChanged, ISaveSource {
		public event PropertyChangedEventHandler PropertyChanged;

		[fsProperty]
		public Money Balance {
			get { return _balance; }
			set {
				if ( _balance == value ) {
					return;
				}
				_balance = value;
				OnPropertyChanged(nameof(Balance));
			}
		}

		Money _balance;

		public FinanceModel() {}

		public FinanceModel(Money balance) {
			Balance = balance;
		}

		void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
