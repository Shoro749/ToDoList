using Data.context;
using Data.models;
using Repositories.Repositories;
using Services.Interfaces;

namespace Services.Services
{
    public class UserService : Service<User>, IUserService
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

        public User GetByName(string name)
        {
            var user = _userRepository.GetByName(name);
            if (user == null) throw new InvalidOperationException($"User not found or password incorrect");
            return user;
        }
    }
}
