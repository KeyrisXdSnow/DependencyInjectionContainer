using System;
using System.Linq;
using Dependency_Injection_Container.exceptions;

namespace Dependency_Injection_Container
{
    public class DependencyProvider
    {
        private DependenciesConfiguration Configuration { get; }
        
        public DependencyProvider(DependenciesConfiguration configuration)
        {
            try
            {
                ValidateConfiguration(configuration);
                Configuration = configuration;
            }
            catch (ValidationException e)
            {
                Console.WriteLine(e.StackTrace);
                throw;
            }
        }

        // TInterface — любой ссылочный тип данных
        // TImplementation — не абстрактный класс, совместимый с TDependency, объект которого может быть создан.
        private static void ValidateConfiguration(DependenciesConfiguration configuration)
        {
            foreach (var keyValuePair in configuration.Dependencies)
            {
                foreach (var implementations in keyValuePair.Value.Where(implementations => implementations.Type.IsAbstract))
                {
                    throw new ValidationException(implementations + " is abstract class");
                }
            }
        }
        
        public  TInterface Resolve<TInterface>()
        where TInterface : class
        {
            return (TInterface) Resolve(Configuration.Dependencies[typeof(TInterface)][0]);
        }

        private object Resolve(Dependency dependency)
        {
            var result = ClassCreator.CreateClass(dependency.Type, Configuration);
            
            if (dependency.LifeCycle == LifeCycle.Singleton) 
                return dependency.GetInstance(result);
            
            return result;
        }
        
    }
}