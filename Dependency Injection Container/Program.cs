using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace Dependency_Injection_Container
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var config = new DependenciesConfiguration();

            config.Register<IRepository, MySqlRepository1>();
            config.Register<IService<IRepository>, ServiceImpl<IRepository>>();
          //  config.Register(typeof(IService<>), typeof(ServiceImpl<>));

            var provider = new DependencyProvider(config);
            var kra = provider.Resolve<IService<IRepository>>();


            
            Console.WriteLine("All OK");
        }
        
        interface IService<TRepository> where TRepository : IRepository
        {
        }

        class ServiceImpl<TRepository> : IService<TRepository> 
            where TRepository : IRepository
        {
            private IRepository Repository;
            public ServiceImpl(TRepository repository)
            {
                Repository = repository;
            }
            
        }
        
        interface IRepository{}

        class MySqlRepository : IRepository
        {
            private int gg;
            public MySqlRepository(int g)
            {
                gg = g;
            }
            
        }
        
        class MySqlRepository1 : IRepository
        {
            private int mm = 100;
            public MySqlRepository1()
            {
            }
            
        }

    }
}