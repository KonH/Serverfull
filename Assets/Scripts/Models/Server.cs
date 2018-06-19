public class Server {
	public float NetworkTime { get; private set; }
	public float ProcessTime { get; private set; }

	public Server(float networkTime, float processTime) {
		NetworkTime = networkTime;
		ProcessTime = processTime;
	}
}
