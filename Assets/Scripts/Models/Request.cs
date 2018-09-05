using System.Collections.Generic;

namespace Serverfull.Models {
	public class Request {
		public RequestId     Id            { get; }
		public ServerType    Type          { get; }
		public User          Owner         { get; }
		public int           WantedNetwork { get; }
		public int           WantedCPU     { get; }
		public int           WantedRAM     { get; }
		public RequestStatus Status        { get; private set; }
		public Server        Origin        { get; private set; }
		public Server        Target        { get; private set; }
		public float         CurProgress   { get; private set; }
		public float         MaxProgress   { get; private set; }

		public float NormalizedProgress => MaxProgress > 0 ? CurProgress / MaxProgress : 0.0f;
		public bool  IsFinished         => Status == RequestStatus.Finished;
		public bool  IsMainRequest      => Type == ServerType.Client;

		public Request(RequestId id, ServerType type, User owner, int wantedNetwork, int wantedCpu, int wantedRam) {
			Id            = id;
			Type          = type;
			Owner         = owner;
			WantedNetwork = wantedNetwork;
			WantedCPU     = wantedCpu;
			WantedRAM     = wantedRam;
		}

		public void ToIncoming(Server origin, Server target, float time) {
			Origin = origin;
			Target = target;
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
			return string.Format(
				"[{0}] Type: {1}, Status: {2}, Target: {3}, Progress: {4}, Owner.Mood: {5:0.00}",
				Id, Type, Status, Target?.Id, NormalizedProgress, Owner.Mood
			);
		}
	}
}
