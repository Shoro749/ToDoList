using Data.context;
using Data.models;
using Repositories.Interfaces;

namespace Repositories.Repositories
{
    public class TaskRepository : Repository<Tasks>, ITaskRepository
    {
        public TaskRepository(DataContext context) : base(context) { }

        public List<Tasks> GetByListId(int id)
        {
            return _context.Tasks.Where(t => t.List.Id == id).ToList();
        }
    }
}
