using Application.DTO;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services.Users
{
    public class IdentityService : IIdentityService
    {
        private readonly IIdentity _identity;
        private readonly ILogger<IdentityService> _logger;

        // constructor 
        public IdentityService(IIdentity identity, ILogger<IdentityService> logger)
        {
            _identity = identity;
            _logger = logger;
        }

        public async Task<bool> LoginAsync(LoginDTO dto)
        {
            bool succeeded = await _identity.LoginAsync(dto);
            if (succeeded)
            {
                _logger.LogInformation("User login succeeded: {Email}", dto.Email);
            }
            else
            {
                _logger.LogWarning("User login failed: {Email}", dto.Email);
            }

            return succeeded;
        }

        public async Task LogoutAsync()
        {
            await _identity.LogoutAsync();
            _logger.LogInformation("User logged out");
        }

        public async Task RegisterUser(RegisterUserDTO dto)
        {
            await _identity.RegisterUser(dto); // Asynchronous
            _logger.LogInformation("User created: {Email} {FirstName} {LastName}", dto.Email, dto.FirstName, dto.LastName);

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
            _logger.LogInformation("User updated: {UserId}", id);
        }
    }
}