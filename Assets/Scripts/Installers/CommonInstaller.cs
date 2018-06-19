using UDBase.Installers;
using UDBase.Controllers.LogSystem;

public class CommonInstaller : UDBaseInstaller {
	public UnityLog.Settings LogSettings;

	public override void InstallBindings() {
		AddEvents();
		AddUnityLogger(LogSettings);
	}
}
