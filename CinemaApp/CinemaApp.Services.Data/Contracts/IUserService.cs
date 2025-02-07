namespace CinemaApp.Services.Data.Contracts
{
    using CinemaApp.Web.ViewModels.Admin.UserManagement;

    public interface IUserService
    {
        Task<IEnumerable<AllUsersViewModel>> GetAllUsersAsync();

        Task<bool> UserExistsByIdAsync(Guid userId);

        Task<bool> AssignUserToRoleAsync(Guid userId, string role);

        Task<bool> RemoveUserRoleAsync(Guid userId, string role);

        Task<bool> DeleteUserAsync(Guid userId);
    }
}
