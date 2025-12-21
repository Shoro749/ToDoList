using Data.models;

namespace Services.Interfaces
{
    public interface ITaskService
    {
        public List<Tasks> GetByListId(int id);
        public List<Tasks> GetByStatus(int id, string status);
    }
}
