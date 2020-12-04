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

            if (Dependencies.ContainsKey(typeof(TInterface)))
            {
                if (typeof(TInterface) != typeof(TImplementation)) 
                    Dependencies[typeof(TInterface)].Add(new Dependency(typeof(TImplementation),lifeCycle));
            }
            else 
                Dependencies.Add(typeof(TInterface), new List<Dependency> { new Dependency(typeof(TImplementation), lifeCycle) } );
        }

        public void Register<TInterface, TImplementation>()
        where TInterface : class
        where TImplementation : class, TInterface
        
        {
            Register<TInterface,TImplementation>(LifeCycle.Instance);
        }
        
    }
}