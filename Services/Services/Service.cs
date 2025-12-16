using Data.context;
using Repositories.Interfaces;
using Repositories.Repositories;
using Services.Interfaces;

namespace Services.Services
{
    public class Service<T> : IService<T> where T : class
    {
        private readonly IRepository<T> _repository;
        public Service(DataContext context) => _repository = new Repository<T>(context);

        public void Add(T item)
        {
            var result = _repository.Add(item);
            if (!result) throw new InvalidOperationException($"{typeof(T)} not added");
        }

        public void Delete(int id)
        {
            bool result = _repository.Delete(id);
            if (!result) throw new InvalidOperationException($"{typeof(T)} not found");
        }

        public List<T> GetAll()
        {
            return _repository.GetAll();
        }

        public T GetById(int id)
        {
            var item = _repository.GetById(id);
            if (item == null) throw new InvalidOperationException($"{typeof(T)} not found");
            return item;
        }

        public void Update(int id, T item)
        {
            var result = _repository.GetById(id);
            if (result == null) throw new InvalidOperationException($"{typeof(T)} not found");

            bool rs = _repository.Update(id, item);
            if (!rs) throw new InvalidOperationException($"Unable to update object {typeof(T)}");
        }
    }
}
