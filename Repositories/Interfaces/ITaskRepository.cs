using Data.models;

namespace Repositories.Interfaces
{
    public interface ITaskRepository
    {
        public List<Tasks> GetByListId(int id);
    }
}
