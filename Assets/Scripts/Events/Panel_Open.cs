namespace Serverfull.Events {
	public enum PanelType {
		None,
		Engineers,
		ServerBuild,
	}

	public struct Panel_Open {
		public PanelType Type { get; }

		public Panel_Open(PanelType type) {
			Type = type;
		}
	}
}
