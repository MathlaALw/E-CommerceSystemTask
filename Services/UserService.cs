using AutoMapper; 
using E_CommerceSystem.Models;
using E_CommerceSystem.Repositories;

namespace E_CommerceSystem.Services
{
    public class UserService : IUserService
    {
        // inject repository
        private readonly IUserRepo _userRepo;

        // AutoMapper
        private readonly IMapper _mapper; 

        public UserService(IUserRepo userRepo, IMapper mapper) // constructor injection
        {
            _userRepo = userRepo;
            _mapper=mapper;
        }

        // Add User
        public void AddUser(UserDTO userDTO)
        {
            // Map DTO to Entity
            var user = _mapper.Map<User>(userDTO);
            _userRepo.AddUser(user);
        }

        // Delete User
        public void DeleteUser(int uid)
        {
            var user = _userRepo.GetUserById(uid);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {uid} not found.");

            _userRepo.DeleteUser(uid);
        }

        // Get All Users
        public IEnumerable<User> GetAllUsers()
        {
            return _userRepo.GetAllUsers();
        }
        public User GetUSer(string email, string password)
        {
            var user = _userRepo.GetUSer(email, password);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }
            return user;
        }
        public User GetUserById(int uid)
        {
            var user = _userRepo.GetUserById(uid);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {uid} not found.");
            return user;
        }
        public void UpdateUser(User user)
        {
            var existingUser = _userRepo.GetUserById(user.UID);
            if (existingUser == null)
                throw new KeyNotFoundException($"User with ID {user.UID} not found.");

            _userRepo.UpdateUser(user);
        }
    }

}

