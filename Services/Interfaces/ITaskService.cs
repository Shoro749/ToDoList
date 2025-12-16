using Data.models;

namespace Services.Interfaces
{
    public interface ITaskService
    {
        public List<Tasks> GetByListId(int id);
    }
}
