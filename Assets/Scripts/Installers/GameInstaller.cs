﻿using System;
using UDBase.Installers;
using Serverfull.Game;
using Serverfull.Controllers;
using Zenject;

namespace Serverfull.Installers {
	public class GameInstaller : UDBaseInstaller {
		public GameSettings  Settings;
		public ServerManager ServerManager;

		public override void InstallBindings() {
			Container.BindInstance(Settings);
			Container.Bind(typeof(TimeController), typeof(ITickable)).To<TimeController>().AsSingle();
			Container.Bind<UserController>().ToSelf().AsSingle();
			Container.Bind(typeof(RequestController), typeof(ITickable)).To<RequestController>().AsSingle().NonLazy();
			Container.Bind(typeof(TransportController), typeof(IInitializable), typeof(IDisposable)).To<TransportController>().AsSingle();
			Container.Bind(typeof(ProcessingController), typeof(IInitializable), typeof(IDisposable)).To<ProcessingController>().AsSingle();
			Container.Bind<ServerController>().ToSelf().AsSingle();
			Container.BindInstance(ServerManager);
		}
	}
}
