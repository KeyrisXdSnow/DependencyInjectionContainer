using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Dependency_Injection_Container
{
    public class ClassCreator
    {
        public static object CreateClass(Type type, DependenciesConfiguration configuration)
        {
            // TODO : поменять тип exception
            if (type.GetConstructors().Length == 0) throw new Exception();

            var constructor = SortConstructors(type.GetConstructors(), configuration)[0];
            var arguments = GetArguments(constructor.GetParameters(), configuration);

            return constructor.Invoke(arguments);
        }
        
        private static object[] GetArguments(IReadOnlyCollection<ParameterInfo> parameterInfos,
            DependenciesConfiguration configuration)
        {
            var arguments = new List<object>(parameterInfos.Count);
            foreach (var parameter in parameterInfos)
            {
                if (configuration.Dependencies.ContainsKey(parameter.ParameterType))
                {
                    // TODO : пока получаем только 1 реализацию, потом исправить на лист
                    var valueType = configuration.Dependencies[parameter.ParameterType][0].Type;
                    arguments.Add(CreateClass(valueType, configuration));
                }
                else
                {
                    arguments.Add(null);
                }
            }

            return arguments.ToArray();
        }

        private static List<ConstructorInfo> SortConstructors
            (IEnumerable<ConstructorInfo> constructorInfos, DependenciesConfiguration configuration)
        {
            var @interfaces = configuration.Dependencies.Keys.ToList();
            var dictionary = new Dictionary<ConstructorInfo, int>();

            foreach (var constructor in constructorInfos)
            {
                var dependencyAmount = constructor.GetParameters()
                    .Count(param => interfaces.Contains(param.ParameterType));
                dictionary.Add(constructor, dependencyAmount);
            }

            return dictionary.OrderBy(pair => pair.Value)
                .ToDictionary(pair => pair.Key, pair => pair.Value).Keys.ToList();
        }
    }
}