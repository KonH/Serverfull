using System;
using System.ComponentModel;
using UDBase.Controllers.SaveSystem;
using FullSerializer;

namespace Serverfull.Models {
	public class TimeModel : INotifyPropertyChanged, ISaveSource {
		public event PropertyChangedEventHandler PropertyChanged;

		[fsProperty]
		DateTime StartTime { get; set; }

		[fsProperty]
		public DateTime Time {
			get { return _time; }
			set {
				if ( _time == value ) {
					return;
				}
				_time = value;
				OnPropertyChanged(nameof(Time));
				var hoursDelta = (Time - StartTime).TotalHours;
				while ( hoursDelta > Hour ) {
					Hour++;
				}
			}
		}

		[fsProperty]
		public int Hour {
			get { return _gameHour; }
			private set {
				if ( _gameHour == value ) {
					return;
				}
				_gameHour = value;
				OnPropertyChanged(nameof(Hour));
			}
		}

		DateTime _time;
		int      _gameHour;

		public TimeModel() { }

		public TimeModel(DateTime time, DateTime startTime) {
			Time = time;
			StartTime = startTime;
		}

		void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
