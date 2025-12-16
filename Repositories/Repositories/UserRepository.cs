using Data.context;
using Data.models;

namespace Repositories.Repositories
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(DataContext context) : base(context) { }

        public bool IsTakenName(string username)
        {
            return _context.Users.Any(u => u.Username == username);
        }

        public User GetByName(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username = username);
        }
    }
}
