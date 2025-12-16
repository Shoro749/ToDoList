using Data.context;
using Data.models;
using Repositories.Repositories;

namespace Services.Services
{
    public class UserService : Service<User>
    {
        private readonly UserRepository _userRepository;
        public UserService(DataContext context) : base (context)
        {
            _userRepository = new UserRepository(context);
        }

        public bool IsTakenName(string name)
        {
            return _userRepository.IsTakenName(name);
        }
    }
}
