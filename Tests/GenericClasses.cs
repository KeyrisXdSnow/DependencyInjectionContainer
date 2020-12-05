namespace Tests
{
    interface IService<TRepository> where TRepository : IRepository
    {
    }

    class ServiceImpl<TRepository> : IService<TRepository> 
        where TRepository : IRepository
    {
        public ServiceImpl(TRepository repository)
        {
        }
    }
    
    
    class ServiceImpl1<TRepository> : IService<TRepository> 
        where TRepository : IRepository
    {
        private IRepository Repository;
        private int g = 10;
        
        public ServiceImpl1(TRepository repository)
        {
            Repository = repository;
        }
    }
}