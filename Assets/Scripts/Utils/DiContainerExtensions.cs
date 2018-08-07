using System;
using Zenject;

namespace Serverfull.Utils {
	public static class DiContainerExtensions {
		public static ConcreteIdBinderNonGeneric BindTickable<T>(this DiContainer container) {
			return container.Bind(typeof(T), typeof(ITickable));
		}

		public static FromBinderNonGeneric BindTickableToSelf<T>(this DiContainer container) {
			return container.BindTickable<T>().To<T>();
		}

		public static ConcreteIdBinderNonGeneric BindInitDispose<T>(this DiContainer container) {
			return container.Bind(typeof(T), typeof(IInitializable), typeof(IDisposable));
		}

		public static FromBinderNonGeneric BindInitDisposeToSelf<T>(this DiContainer container) {
			return container.BindInitDispose<T>().To<T>();
		}
	}
}
