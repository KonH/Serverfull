public struct RequestNewStatus {
	public Request Request { get; private set; }
	public RequestStatus Status { get; private set; }

	public RequestNewStatus(Request request) {
		Request = request;
		Status = request.Status;
	}

	public override string ToString() {
		return string.Format("RequestNewStatus: {0}", Request);
	}
}
