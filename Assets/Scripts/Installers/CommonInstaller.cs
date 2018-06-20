using UDBase.Installers;
using UDBase.Controllers.LogSystem;

namespace Serverfull.Installers {
	public class CommonInstaller : UDBaseInstaller {
		public UnityLog.Settings LogSettings;

		public override void InstallBindings() {
			AddEvents();
			AddUnityLogger(LogSettings);
		}
	}
}