using Data.models;

namespace Repositories.Interfaces
{
    public interface IUserRepository
    {
        public bool IsTakenName(string name);
        public User GetByName(string name);
    }
}
