namespace Services.Interfaces
{
    public interface IService<T> where T : class
    {
        List<T> GetAll();
        T GetById(int id);
        void Add(T item);
        void Update(int id, T item);
        void Delete(int id);
    }
}
