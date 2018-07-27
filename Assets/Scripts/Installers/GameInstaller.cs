using UDBase.Installers;
using Serverfull.Game;
using Serverfull.Utils;
using Serverfull.Controllers;

namespace Serverfull.Installers {
	public class GameInstaller : UDBaseInstaller {
		public GameSettings  Settings;
		public ServerManager ServerManager;
		public ServerBuilder ServerBuilder;
		
		public override void InstallBindings() {
			Container.BindInstance(Settings);
			Container.Bind<GameRules>().ToSelf().AsSingle();
			Container.BindTickableToSelf<TimeController>().AsSingle();
			Container.Bind<ClientController>().ToSelf().AsSingle();
			Container.Bind<UserController>().ToSelf().AsSingle();
			Container.BindTickableToSelf<RequestController>().AsSingle();
			Container.BindTickableToSelf<RequestSpawnController>().AsSingle().NonLazy();
			Container.BindInitDisposeToSelf<TransportController>().AsSingle();
			Container.BindInitDisposeToSelf<ProcessingController>().AsSingle();
			Container.Bind<ServerController>().ToSelf().AsSingle();
			Container.Bind<ServerBuildController>().ToSelf().AsSingle().NonLazy();
			Container.BindInitDisposeToSelf<FinanceController>().AsSingle();
			Container.Bind<UpgradeController>().ToSelf().AsSingle();
			Container.BindInitDisposeToSelf<StatusController>().AsSingle();
			Container.BindInitDisposeToSelf<MessageController>().AsSingle().NonLazy();
			Container.BindInitDisposeToSelf<ClientMoodController>().AsSingle().NonLazy();
			Container.BindInitDisposeToSelf<TutorialController>().AsSingle().NonLazy();
			Container.BindInstance(ServerManager);
			Container.BindInstance(ServerBuilder);
		}
	}
}
