using UnityEngine;
using UDBase.Installers;
namespace Serverfull.Installers {
	public class MenuInstaller : UDBaseInstaller {

		public override void InstallBindings() {
			AddDirectSceneLoader();
		}
	}
}
