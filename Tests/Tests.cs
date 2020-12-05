using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dependency_Injection_Container;
using Dependency_Injection_Container.exceptions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void ValidationTest()
        {
            var dependencies = new DependenciesConfiguration();
            dependencies.Register<IService, ServiceImpl>();
            dependencies.Register<IRepository, RepositoryImpl>();

            var provider = new DependencyProvider(dependencies);
            Assert.True(true);
        }

        [Test]
        public void InvalidValidationTest()
        {
            var dependencies = new DependenciesConfiguration();
            dependencies.Register<IService, ServiceImpl>();
            dependencies.Register<IService, AbstractService>();
            dependencies.Register<IRepository, RepositoryImpl>();

            Assert.Throws<ValidationException>(delegate
            {
                var provider = new DependencyProvider(dependencies);
            });
        }

        [Test]
        public void AsSelfDependencyTest()
        {
            var dependencies = new DependenciesConfiguration();
            dependencies.Register<RepositoryImpl, RepositoryImpl>();

            var provider = new DependencyProvider(dependencies);
            RepositoryImpl repository = provider.Resolve<RepositoryImpl>();
            Assert.NotNull(repository);
            Assert.IsInstanceOf(typeof(RepositoryImpl),repository);
        }

        [Test]
        public void RecursiveDependencyInjectionTest()
        {
            // конфигурация и использование контейнера
            var dependencies = new DependenciesConfiguration();
            dependencies.Register<IService, ServiceImpl>();
            dependencies.Register<IRepository, RepositoryImpl>();

            var provider = new DependencyProvider(dependencies);
            
            var service1 = provider.Resolve<IService>();

            Assert.NotNull(service1);
        }

        [Test]
        public void RecursiveDependencyInjectionAsSelfTest()
        {
            var dependencies = new DependenciesConfiguration();
            dependencies.Register<ServiceImpl, ServiceImpl>();
            dependencies.Register<IRepository, RepositoryImpl>();

            var provider = new DependencyProvider(dependencies);
            var serviceImpl = provider.Resolve<ServiceImpl>();

            Assert.NotNull(serviceImpl);
        }

        [Test]
        public void SingletonTest()
        {
            var dependencies = new DependenciesConfiguration();
            dependencies.Register<IService, ServiceImpl>(LifeCycle.Singleton);
            dependencies.Register<IRepository, RepositoryImpl>(LifeCycle.Singleton);

            var provider = new DependencyProvider(dependencies);
            
            var service1 = provider.Resolve<IService>();
            var service2 = provider.Resolve<IService>();

            Assert.AreEqual(service1, service2);
        }

        [Test]
        public void InstancePerDependencyTest()
        {
            var dependencies = new DependenciesConfiguration();
            dependencies.Register<IService, ServiceImpl>(LifeCycle.Instance);
            dependencies.Register<IRepository, RepositoryImpl>(LifeCycle.Singleton);

            var provider = new DependencyProvider(dependencies);

            var service1 = provider.Resolve<IService>();
            var service2 = provider.Resolve<IService>();

            Assert.AreNotEqual(service1, service2);
        }


        [Test]
        public void GetListDependenciesTest()
        {
            var dependencies = new DependenciesConfiguration();
            dependencies.Register<IService, ServiceImpl>();
            dependencies.Register<IService, ServiceImpl1>();

            var provider = new DependencyProvider(dependencies);
            var services = provider.Resolve<IEnumerable<IService>>().ToList();

            Assert.NotNull(services);
            Assert.True(services.Count == 2);
            Assert.IsInstanceOf(typeof(ServiceImpl),services[0]);
            Assert.IsInstanceOf(typeof(ServiceImpl1),services[1]);
        }

        [Test]
        public void GenericConfigurationTest()
        {
            var dependencies = new DependenciesConfiguration();
            dependencies.Register<IRepository, MySqlRepository>(LifeCycle.Singleton);
            dependencies.Register<IService<IRepository>, ServiceImpl<IRepository>>();
            dependencies.Register(typeof(IService<>), typeof(ServiceImpl<>));

            var provider = new DependencyProvider(dependencies);
            Assert.True(true);
        }
        
        
        [Test]
        public void GetGenericClassTest()
        {
            var dependencies = new DependenciesConfiguration();
           
            dependencies.Register<IRepository, MySqlRepository>();
            dependencies.Register<IService<IRepository>, ServiceImpl<IRepository>>();
            dependencies.Register(typeof(IService<>), typeof(ServiceImpl<>));

            var provider = new DependencyProvider(dependencies);
            
            var service = provider.Resolve<IService<IRepository>>();
            
            Assert.NotNull(service);
            Assert.IsInstanceOf(typeof(ServiceImpl<IRepository>),service);
        }
        
        [Test]
        public void GetGenericListTest()
        {
            var dependencies = new DependenciesConfiguration();
           
            dependencies.Register<IRepository, MySqlRepository>();
            dependencies.Register<IService<IRepository>, ServiceImpl<IRepository>>();
            dependencies.Register<IService<IRepository>, ServiceImpl1<IRepository>>();
            
            var provider = new DependencyProvider(dependencies);
            
            var serviceList = provider.Resolve<IEnumerable<IService<IRepository>>>().ToList();
            
            Assert.NotNull(serviceList);
            Assert.True( serviceList.Count == 2);
            
            Assert.IsInstanceOf(typeof(ServiceImpl<IRepository>),serviceList[0]);
            Assert.IsInstanceOf(typeof(ServiceImpl1<IRepository>),serviceList[1]);
        }
        
        
        [Test]
        public void GetNameDependencyTest()
        {
            var dependencies = new DependenciesConfiguration();
           
            dependencies.Register<IService, ServiceImpl>(Key.Second);
            dependencies.Register<IService, ServiceImpl1>(Key.First);
            
            var provider = new DependencyProvider(dependencies);
            
            var service = provider.Resolve<IService>(Key.Second);
            
            Assert.NotNull(service);
            Assert.IsInstanceOf(typeof(ServiceImpl),service);
        }
        
        [Test]
        public void GetGenericNameDependencyTest()
        {
            var dependencies = new DependenciesConfiguration();
           
            dependencies.Register<IRepository,RepositoryImpl>(Key.Second);
            
            dependencies.Register<IService, SomeAnotherService>();
            dependencies.Register<IService, ServiceImpl1>(Key.First);
            
            var provider = new DependencyProvider(dependencies);
            
            var service1 = provider.Resolve<IService>();
            
            Assert.NotNull(service1);
            Assert.IsInstanceOf(typeof(SomeAnotherService),service1);
            Assert.IsNotNull(service1.GetTestFieldValue());
            
            
        }
        
        [Test]
        public void GetGenericNameDependencyTest2()
        {
            var dependencies = new DependenciesConfiguration();
           
            dependencies.Register<IRepository,RepositoryImpl>();
            dependencies.Register<IService<IRepository>, ServiceImpl<IRepository>>();
            dependencies.Register<IService<IRepository>, ServiceImpl1<IRepository>>(Key.First);
            
            var provider = new DependencyProvider(dependencies);
            
            var service2 = provider.Resolve<IService<IRepository>>(Key.First);
            
            Assert.NotNull(service2);
            Assert.IsInstanceOf(typeof(ServiceImpl1<IRepository>),service2);
        }
        
        
    }
}