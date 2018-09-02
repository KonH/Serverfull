using UnityEngine;
using UDBase.Installers;
using Serverfull.Game;
using Serverfull.Utils;
using Serverfull.Common;
using Serverfull.Controllers;

namespace Serverfull.Installers {
	public class GameInstaller : UDBaseInstaller {
		[Header("Settings")]
		public GameSettings    Settings;
		
		[Header("Scene Managers")]
		public ServerManager   ServerManager;
		public ServerBuilder   ServerBuilder;
		public EngineerManager EngineerManager;
		
		public override void InstallBindings() {
			// base
			Container.BindInstance(Settings);
			Container.Bind<GameRules>().ToSelf().AsSingle();
			
			// single
			Container.BindTickableToSelf<TimeController>().AsSingle();
			Container.Bind<FinanceController>().ToSelf().AsSingle();
			Container.BindInitDisposeToSelf<TutorialController>().AsSingle().NonLazy();
			Container.BindInitDisposeToSelf<StatusController>().AsSingle();
			Container.BindInitDisposeToSelf<MessageController>().AsSingle().NonLazy();
			Container.Bind<UserController>().ToSelf().AsSingle();
			
			// client
			Container.BindInitDisposeToSelf<ClientController>().AsSingle();
			Container.BindInitDisposeToSelf<ClientMoodController>().AsSingle().NonLazy();
			Container.BindInitDisposeToSelf<ClientSpawnController>().AsSingle().NonLazy();
			Container.Bind<ClientGenerator>().ToSelf().AsSingle();

			// request
			Container.BindTickableToSelf<RequestController>().AsSingle();
			Container.BindTickableToSelf<RequestSpawnController>().AsSingle().NonLazy();
			Container.BindInitDisposeToSelf<TransportController>().AsSingle();
			Container.BindInitDisposeToSelf<ProcessingController>().AsSingle();
			
			// server
			Container.BindInitDisposeToSelf<ServerController>().AsSingle();
			Container.Bind<ServerBuildController>().ToSelf().AsSingle().NonLazy();
			Container.Bind<UpgradeController>().ToSelf().AsSingle();
			Container.BindTickableToSelf<BreakController>().AsSingle().NonLazy();
			
			// engineer
			Container.BindInitDisposeToSelf<EngineerController>().AsSingle().NonLazy();
			Container.BindInitDisposeToSelf<EngineerSpawnController>().AsSingle().NonLazy();
			Container.Bind<EngineerGenerator>().ToSelf().AsSingle();
			
			// scene managers
			Container.BindInstance(ServerManager);
			Container.BindInstance(ServerBuilder);
			Container.BindInstance(EngineerManager);
		}
	}
}
