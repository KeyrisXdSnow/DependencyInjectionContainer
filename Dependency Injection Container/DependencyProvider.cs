using System;
using System.Collections;
using System.Collections.Generic;
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
                foreach (var implementations in keyValuePair.Value.Where(implementations =>
                    implementations.Type.IsAbstract))
                {
                    throw new ValidationException(implementations + " is abstract class");
                }
            }
        }

        public TInterface Resolve<TInterface>()
            where TInterface : class
        {
            return (TInterface) Resolve(typeof(TInterface));
        }

        private object Resolve(Type interfaceType)
        {
            if (interfaceType.GetGenericTypeDefinition() == typeof(List<>))
            {
                var @interface = interfaceType.GetGenericArguments()[0];
                return ClassCreator.CreateClassIEnumerable(@interface, Configuration.Dependencies[@interface],
                    Configuration);
            }

            return ClassCreator.CreateClass(Configuration.Dependencies[interfaceType][0], Configuration);
        }

        // private object Resolve(Dependency dependency)
        // {
        //     var result = ClassCreator.CreateClass(dependency, Configuration);
        //     return dependency.LifeCycle == LifeCycle.Singleton ? dependency.GetInstance(result) : result;
        // }
        //
        // // private IEnumerable<object> ResolveIEnumerable(Type @interface, IEnumerable<Dependency> dependencies)
        // // {
        // //     var list = (IList) Activator.CreateInstance(typeof(List<>).MakeGenericType(@interface));
        // //     
        // //     foreach (var dependency in dependencies)
        // //     {
        // //         list.Add(Resolve(dependency));
        // //     }
        // //     return (IEnumerable<object>) list;
        // // }
        //
        //
        // private IEnumerable<object> Resolve(IEnumerable<Dependency> dependencies)
        // {
        //     return dependencies.Select(dependency => Resolve(dependency));
        // }
    }
}