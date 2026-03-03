using Application.DTO;

namespace Application.Interfaces
{
    public interface IIdentity
    {
        Task<bool> LoginAsync(LoginDTO dto);
        Task LogoutAsync();
        Task RegisterUser(RegisterUserDTO dto);
        Task<List<UserDetailDTO>> GetAllUsers();
        Task<UserDetailDTO?> GetUserById(int id);
        Task UpdateUser(int id, UserUpdateDTO dto);

        /// <summary>
        /// Gets the current authenticated user's profile, or null if not authenticated.
        /// </summary>
        Task<UserDetailDTO?> GetCurrentUserAsync();
    }
}