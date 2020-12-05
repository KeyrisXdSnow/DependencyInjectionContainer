using System;
using System.Collections.Generic;
using System.Linq;
using Dependency_Injection_Container;
using Dependency_Injection_Container.exceptions;

namespace DependencyInjectionContainerLib
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
        
        public TInterface Resolve<TInterface>()
            where TInterface : class
        {
            return (TInterface) Resolve(typeof(TInterface));
        }

        public TInterface Resolve<TInterface>(Key key)
            where TInterface : class
        {
            return (TInterface) Resolve(typeof(TInterface), key);
        }

        private static void ValidateConfiguration(DependenciesConfiguration configuration)
        {
            foreach (var implementations in configuration.Dependencies
                .SelectMany(
                    keyValuePair => keyValuePair.Value.Where(implementations => implementations.Type.IsAbstract)))
            {
                throw new ValidationException(implementations + " is abstract class");
            }
        }
        
        private object Resolve(Type interfaceType, Key key)
        {
            if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var @interface = interfaceType.GetGenericArguments()[0];
                return ClassCreator.CreateClassIEnumerable(@interface, Configuration.Dependencies[@interface],
                    Configuration);
            }

            return ClassCreator.CreateClass(
                Configuration.Dependencies[interfaceType].Find(dependency => dependency.Key == key ), 
                Configuration);
        }
        
        private object Resolve(Type interfaceType)
        {
            if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var @interface = interfaceType.GetGenericArguments()[0];
                return ClassCreator.CreateClassIEnumerable(@interface, Configuration.Dependencies[@interface],
                    Configuration);
            }

            return ClassCreator.CreateClass(Configuration.Dependencies[interfaceType][0], Configuration);
        }
    }
}