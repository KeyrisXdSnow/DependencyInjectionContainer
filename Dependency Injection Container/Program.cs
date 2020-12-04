using System;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace Dependency_Injection_Container
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var config = new DependenciesConfiguration();
            
            //config.Register<IService,ServiceImpl>();
            config.Register<IService,ServiceRmpl>(LifeCycle.Singleton);
            config.Register<IRepository,RepositoryImpl>();
            config.Register<IClass,ClassImpl>();
            
            
            DependencyProvider provider = new DependencyProvider(config);
            //var serviceImpl = provider.Resolve<IService>();

            var s1 = provider.Resolve<IService>();
            var s2 = provider.Resolve<IService>();

            bool flag = s1.Equals(s2);
            
            Console.WriteLine("All OK");
        }
        
        
        interface IService {}
        class ServiceImpl : IService
        {
            private IRepository Repository;
            private int IntValue;
            private string StringValue;
            
            public ServiceImpl(IRepository repository, int intValue, string stringValue) // ServiceImpl зависит от IRepository
            {
                Repository = repository;
                IntValue = intValue;
                StringValue = stringValue;

            }
        }
        
        class ServiceRmpl : IService
        {
            private int y = 10;
            public ServiceRmpl(IRepository repository) // ServiceImpl зависит от IRepository
            {
                
            }
        }
        interface IRepository{}
        class RepositoryImpl : IRepository
        {
            public string Rep = "repositoryImpl";
            private IClass _class;

            public RepositoryImpl(IClass @class)
            {
                _class = @class;
            } // может иметь свои зависимости, опустим для простоты
        }
        
        interface IClass{}
        class ClassImpl : IClass
        {
            public string cl = "classImpl";
            
            public ClassImpl(){} // может иметь свои зависимости, опустим для простоты
        }

    }
}