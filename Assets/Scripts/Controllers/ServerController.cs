public class ServerController {
	Server _instance;

	public ServerController(GameSettings settings) {
		_instance = new Server(settings.NetworkTime, settings.ProcessTime);
	}

	public Server GetServerForRequest(Request request) {
		return _instance;
	}
}
