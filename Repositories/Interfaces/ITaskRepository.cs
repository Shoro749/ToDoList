using Data.models;

namespace Repositories.Interfaces
{
    public interface ITaskRepository
    {
        public List<Tasks> GetByListId(int id);
        public List<Tasks> GetByStatus(int id, string status);
    }
}
