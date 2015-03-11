using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Utility.MyUnityHelper
{
    public static class UnityHelper
    {
        private static IUnityContainer _container = null;
        public static void Init()
        {
            if (_container == null) {
                _container = new UnityContainer();
                _container.LoadConfiguration();
            }
        }

        public static T Resolve<T>()
        {
            if (_container == null) UnityHelper.Init();
            return _container.Resolve<T>();
        }

        public static T Resolve<T>(params ResolverOverride[] overrides)
        {
            if (_container == null) UnityHelper.Init();
            return _container.Resolve<T>(overrides);
        }

    }
}
