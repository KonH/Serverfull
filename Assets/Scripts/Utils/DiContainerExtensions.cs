using System;
using Zenject;

namespace Serverfull.Utils {
	public static class DiContainerExtensions {
		public static FromBinderNonGeneric BindToSelf<T0, T1>(this DiContainer container) {
			return container.Bind(typeof(T0), typeof(T1)).To<T0>();
		}

		public static FromBinderNonGeneric BindToSelf<T0, T1, T2>(this DiContainer container) {
			return container.Bind(typeof(T0), typeof(T1), typeof(T2)).To<T0>();
		}

		public static FromBinderNonGeneric BindToSelf<T0, T1, T2, T3>(this DiContainer container) {
			return container.Bind(typeof(T0), typeof(T1), typeof(T2), typeof(T3)).To<T0>();
		}

		public static FromBinderNonGeneric BindTickableToSelf<T>(this DiContainer container) {
			return container.BindToSelf<T, ITickable>();
		}

		public static FromBinderNonGeneric BindInitDisposeToSelf<T>(this DiContainer container) {
			return container.BindToSelf<T, IInitializable, IDisposable>();
		}
	}
}
