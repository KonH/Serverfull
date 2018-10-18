using Serverfull.Models;
using UnityWeld.Binding;

namespace Serverfull.ViewModels {
	[Adapter(typeof(Money), typeof(string))]
	public class MoneyToStringAdapter : IAdapter {
		public object Convert(object valueIn, AdapterOptions options) {
			var money = (Money)valueIn;
			return "$" + money.Value;
		}
	}
}
