using Data.context;
using Data.models;
using Repositories.Interfaces;
using Repositories.Repositories;
using Services.Interfaces;

namespace Services.Services
{
    public class TaskService : Service<Tasks>, ITaskService
    {
        private readonly ITaskRepository _repository;
        public TaskService(DataContext context) : base(context)
        {
            _repository = new TaskRepository(context);
        }

        public List<Tasks> GetByListId(int id)
        {
            return _repository.GetByListId(id);
        }

        public List<Tasks> GetByStatus(int id, string status)
        {
            return _repository.GetByStatus(id, status);
        }
    }
}
