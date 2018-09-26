using System.Collections.Generic;
using UDBase.Controllers.SaveSystem;

namespace Serverfull.Controllers {
	public interface ISavable {
		void Save(ISave save);
		void Load(ISave save);
	}

	public class SaveController {
		readonly ISave          _save;
		readonly List<ISavable> _controllers;

		public SaveController(ISave save, List<ISavable> controllers) {
			_save = save;
			_controllers = controllers;
			Load();
		}

		public void Save() {
			foreach ( var controller in _controllers ) {
				controller.Save(_save);
			}
			_save.Flush();
		}

		public void Load() {
			foreach ( var controller in _controllers ) {
				controller.Load(_save);
			}
		}
	}
}
