using Data.models;

namespace Services.Interfaces
{
    public interface IUserService
    {
        public bool IsTakenName(string name);
        public User GetByName(string name);
    }
}
