using System;
using UDBase.Installers;
using Zenject;

public class GameInstaller : UDBaseInstaller {
	public GameSettings Settings;

	public override void InstallBindings() {
		Container.BindInstance(Settings);
		Container.Bind(typeof(TimeController), typeof(ITickable)).To<TimeController>().AsSingle();
		Container.Bind(typeof(RequestController), typeof(ITickable)).To<RequestController>().AsSingle().NonLazy();
		Container.Bind(typeof(TransportController), typeof(IInitializable), typeof(IDisposable)).To<TransportController>().AsSingle();
		Container.Bind(typeof(ProcessingController), typeof(IInitializable), typeof(IDisposable)).To<ProcessingController>().AsSingle();
		Container.Bind<ServerController>().ToSelf().AsSingle();
	}
}
