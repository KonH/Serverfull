public class Request {
	public long Index { get; }
	public RequestStatus Status { get; private set; } 
	public Server Target { get; private set; }
	public float CurProgress { get; private set; }
	public float MaxProgress { get; private set; }
	public float NormalizedProgress => MaxProgress > 0 ? CurProgress / MaxProgress : 0.0f;
	public bool IsFinished => Status == RequestStatus.Finished;

	public Request(long index) {
		Index = index;
	}

	public void ToIncoming(Server server, float time) {
		Target = server;
		AssingStatus(RequestStatus.Incoming, time);
	}

	public void ToProcessing(float time) {
		AssingStatus(RequestStatus.Processing, time);
	}

	public void ToOutgoing(float time) {
		AssingStatus(RequestStatus.Outgoing, time);
	}

	public void ToFinished() {
		AssingStatus(RequestStatus.Finished, 0.0f);
	}

	void AssingStatus(RequestStatus status, float maxProgress) {
		Status = status;
		CurProgress = 0.0f;
		MaxProgress = maxProgress;
	}

	public bool UpdateProgress(float inc) {
		CurProgress += inc;
		return CurProgress >= MaxProgress;
	}

	public override string ToString() {
		return string.Format("[{0}] Status: {1}, Progress: {2}", Index, Status, NormalizedProgress);
	}

	public override int GetHashCode() {
		return Index.GetHashCode();
	}
}
