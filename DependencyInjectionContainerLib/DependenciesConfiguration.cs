using System;
using System.Collections.Generic;
using Dependency_Injection_Container;

namespace DependencyInjectionContainerLib
{
    public class DependenciesConfiguration
    {
        internal Dictionary<Type, List<Dependency>> Dependencies { get; }

        public DependenciesConfiguration()
        {
            Dependencies = new Dictionary<Type, List<Dependency>>();
        }

        public void Register<TInterface, TImplementation>(LifeCycle lifeCycle)
            where TInterface : class
            where TImplementation : class, TInterface

        {
            Register(typeof(TInterface), typeof(TImplementation), lifeCycle, Key.None);
        }

        public void Register<TInterface, TImplementation>(LifeCycle lifeCycle, Key key)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            Register(typeof(TInterface), typeof(TImplementation), lifeCycle, key);
        }
        public void Register<TInterface, TImplementation>(Key key)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            Register(typeof(TInterface), typeof(TImplementation), LifeCycle.Instance, key);
        }

        public void Register<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : class, TInterface
        {
            Register(typeof(TInterface), typeof(TImplementation), LifeCycle.Instance, Key.None);
        }

        public void Register(Type @interface, Type type)
        {
            Register(@interface, type, LifeCycle.Instance, Key.None);
        }

        public void Register(Type @interface, Type type, LifeCycle lifeCycle, Key key)
        {
            if (Dependencies.ContainsKey(@interface))
            {
                if (@interface != type)
                    Dependencies[@interface].Add(new Dependency(type, lifeCycle, key));
            }
            else
                Dependencies.Add(@interface, new List<Dependency> {new Dependency(type, lifeCycle, key)});
        }
    }
}