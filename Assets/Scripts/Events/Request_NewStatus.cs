using Serverfull.Models;

namespace Serverfull.Events {
	public struct Request_NewStatus {
		public RequestId     Id            { get; private set; }
		public bool          IsMainRequest { get; private set; }
		public RequestStatus NewStatus     { get; private set; }

		string _info;

		public Request_NewStatus(Request request) {
			Id            = request.Id;
			IsMainRequest = request.IsMainRequest;
			NewStatus     = request.Status;
			_info         = string.Format("Request_NewStatus: {0}", request);
		}

		public override string ToString() {
			return _info;
		}
	}
}
