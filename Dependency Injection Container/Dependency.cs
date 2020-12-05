using System;

namespace Dependency_Injection_Container
{
    public class Dependency
    {
        public Type Type { get; }
        public LifeCycle LifeCycle { get; }

        // singleton object
        private object _instance;
        private static readonly object Locker = new object();

        public Dependency(Type type, LifeCycle lifeCycle)
        {
            Type = type;
            LifeCycle = lifeCycle;
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