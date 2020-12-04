using System;

namespace Dependency_Injection_Container
{
    public class Dependency
    {
        public Type Type { get; }
        public LifeCycle LifeCycle { get; }

        // singleton object
        private object _instance;

        public Dependency(Type type, LifeCycle lifeCycle)
        {
            Type = type;
            LifeCycle = lifeCycle;
        }

        public object GetInstance( object @object)
        {
            if (_instance == null)
                _instance = @object;
            return _instance;
        }
    }
}