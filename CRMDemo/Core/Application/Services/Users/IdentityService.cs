using Application.DTO;
using Application.Interfaces;

namespace Application.Services.Users
{
    public class IdentityService : IIdentityService
    {
        private readonly IIdentity _identity;

        // constructor 
        public IdentityService(IIdentity identity)
        {
            _identity = identity;
        }

        public async Task<bool> LoginAsync(LoginDTO dto)
        {
            return await _identity.LoginAsync(dto);
        }

        public async Task LogoutAsync()
        {
            await _identity.LogoutAsync();
        }

        public async Task RegisterUser(RegisterUserDTO dto)
        {
            await _identity.RegisterUser(dto); // Asynchronous

        }

        public async Task<List<UserDetailDTO>> GetAllUsers()
        {
            return await _identity.GetAllUsers();
        }

        public async Task<UserDetailDTO?> GetUserById(int id)
        {
            return await _identity.GetUserById(id);
        }

        public async Task UpdateUser(int id, UserUpdateDTO dto)
        {
            await _identity.UpdateUser(id, dto);
        }
    }
}