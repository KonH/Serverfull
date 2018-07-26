namespace Serverfull.Events {
	public struct Notification_New {
		public string Message { get; }

		public Notification_New(string message) {
			Message = message;
		}
	}
}
