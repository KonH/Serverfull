namespace Serverfull.Models {
	public class Engineer {
		public EngineerId Id      { get; }
		public float      FixTime { get; }

		public Engineer(EngineerId id, float fixTime) {
			Id      = id;
			FixTime = fixTime;
		}
	}
}
