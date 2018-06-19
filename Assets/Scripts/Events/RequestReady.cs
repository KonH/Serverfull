public struct RequestReady {
	public Request Request { get; private set; }
	public RequestStatus Status { get; private set; }

	public RequestReady(Request request) {
		Request = request;
		Status = request.Status;
	}

	public override string ToString() {
		return string.Format("RequestReady: {0}", Request);
	}
}
