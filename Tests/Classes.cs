using System;

namespace Tests
{

    interface IService
    {
        object GetTestFieldValue();
    }
    class ServiceImpl : IService
    {
        public IRepository Repository { get; }

        public ServiceImpl(IRepository repository)
        {
            Repository = repository;
        }

        public object GetTestFieldValue()
        {
            return Repository;
        }
    }
    class ServiceImpl1 : IService
    {
        private IRepository Repository;
        
        public ServiceImpl1(IRepository repository)
        {
            Repository = repository;
        }


        public object GetTestFieldValue()
        {
            return Repository;
        }
    }

    abstract class AbstractService : IService
    {
        public object GetTestFieldValue()
        {
            throw new NotImplementedException();
        }
    }

    public interface IRepository{}
    class RepositoryImpl : IRepository
    {
        private int g = 10;
        
        public RepositoryImpl()
        {
        }
    }
    class MySqlRepository : IRepository
    {
        public MySqlRepository()
        {
        }
    }
}