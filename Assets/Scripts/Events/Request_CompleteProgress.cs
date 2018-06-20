using Serverfull.Models;

namespace Serverfull.Events {
	public struct Request_CompleteProgress {
		public RequestId     Id              { get; private set; }
		public RequestStatus CompletedStatus { get; private set; }

		string _info;

		public Request_CompleteProgress(Request request) {
			Id              = request.Id;
			CompletedStatus = request.Status;
			_info           = string.Format("Request_CompleteProgress: {0}", Id);
		}

		public override string ToString() {
			return _info;
		}
	}
}
