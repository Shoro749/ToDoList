namespace Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        List<T> GetAll();
        T GetById(int id);
        bool Add(T item);
        bool Update(int id, T item);
        bool Delete(int id);
    }
}
