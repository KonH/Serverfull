using Serverfull.Models;

namespace Serverfull.Events {
	public struct Server_New {
		public ServerId Id   { get; }
		public int      PosX { get; }
		public int      PosY { get; }

		public Server_New(ServerId id, int posX, int posY) {
			Id   = id;
			PosX = posX;
			PosY = posY;
		}
	}
}