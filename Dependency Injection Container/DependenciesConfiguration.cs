using System;
using System.Collections.Generic;

namespace Dependency_Injection_Container
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
            Register(typeof(TInterface),typeof(TImplementation),lifeCycle);
        }

        public void Register<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : class, TInterface

        {
            Register(typeof(TInterface),typeof(TImplementation),LifeCycle.Instance);
        }

        public void Register(Type @interface, Type type)
        {
            Register(@interface,type,LifeCycle.Instance);
        }

        public void Register(Type @interface, Type type, LifeCycle lifeCycle)
        {
            if (Dependencies.ContainsKey(@interface))
            {
                if (@interface != type)
                    Dependencies[@interface].Add(new Dependency(type, lifeCycle));
            }
            else
                Dependencies.Add(@interface, new List<Dependency> {new Dependency(type, lifeCycle)});
        }
    }
}