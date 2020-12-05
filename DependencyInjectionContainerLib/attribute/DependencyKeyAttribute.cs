using DependencyInjectionContainerLib;

namespace Dependency_Injection_Container
{
    
    [System.AttributeUsage(System.AttributeTargets.Parameter)]  
    public class DependencyKey : System.Attribute
    {
        public Key Key { get; }

        public DependencyKey(Key key)
        {
            Key = key;
        }
    }
}