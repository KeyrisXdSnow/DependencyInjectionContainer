using System;
using Dependency_Injection_Container;

namespace DependencyInjectionContainerLib
{
    public class Dependency
    {
        public Type Type { get; }
        public LifeCycle LifeCycle { get; }

        public Key Key { get; }

        // singleton object
        private object _instance;
        private static readonly object Locker = new object();
        
        public Dependency(Type type, LifeCycle lifeCycle, Key key)
        {
            Type = type;
            LifeCycle = lifeCycle;
            Key = key;
        }

        public object GetInstance(object @object)
        {
            lock (Locker)
            {
                return _instance ?? (_instance = @object);
            }
        }
    }
}