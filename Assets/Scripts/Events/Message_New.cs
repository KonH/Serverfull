using System;
using Serverfull.Models;

namespace Serverfull.Events {
	public struct Message_New {
		public Message Message    { get; }
		public Action  OnPositive { get; }
		public Action  OnNegative { get; }

		public Message_New(Message message, Action onPositive, Action onNegative) {
			Message    = message;
			OnPositive = onPositive;
			OnNegative = onNegative;
		}
	}
}