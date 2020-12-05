﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Dependency_Injection_Container
{
    public static class ClassCreator
    {
        public static object CreateClass(Dependency dependency, DependenciesConfiguration configuration)
        {
            var type = dependency.Type;

            // TODO : поменять тип exception
            // TODO : находить не все конструкторы а только public / пофиксить флагами
            if (type.GetConstructors().Length == 0) throw new Exception();

            var constructor = SortConstructors(type.GetConstructors(), configuration)[0];
            var arguments = GetArguments(constructor.GetParameters(), configuration);

            var result =  constructor.Invoke(arguments);
            return dependency.LifeCycle == LifeCycle.Singleton ? dependency.GetInstance(result) : result;
        }

        public static IEnumerable<object> CreateClassIEnumerable(Type @interface, List<Dependency> dependencies,
            DependenciesConfiguration configuration)
        {
            var list = (IList) Activator.CreateInstance(typeof(List<>).MakeGenericType(@interface));

            foreach (var dependency in dependencies)
            {
                list.Add(CreateClass(dependency, configuration));
            }

            return (IEnumerable<object>) list;
        }

        private static object[] GetArguments(IReadOnlyCollection<ParameterInfo> parameterInfos,
            DependenciesConfiguration configuration)
        {
            var arguments = new List<object>(parameterInfos.Count);
            foreach (var parameter in parameterInfos)
            {
                if (parameter.ParameterType.IsGenericType)
                {
                    var @interface = parameter.ParameterType.GetGenericArguments()[0];
                    arguments.Add(CreateClassIEnumerable(@interface,configuration.Dependencies[@interface],configuration));
                }
                else
                {
                    if (configuration.Dependencies.ContainsKey(parameter.ParameterType))
                    {
                        // TODO : пока получаем только 1 реализацию, потом исправить на лист
                        var valueType = configuration.Dependencies[parameter.ParameterType][0];
                        arguments.Add(CreateClass(valueType, configuration));
                    }
                    else
                    {
                        arguments.Add(null);
                    }
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