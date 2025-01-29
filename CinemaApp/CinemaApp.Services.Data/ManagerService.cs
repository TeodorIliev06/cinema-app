namespace CinemaApp.Services.Data
{
    using CinemaApp.Data.Models;
    using CinemaApp.Data.Repositories.Contracts;
    using CinemaApp.Services.Data.Contracts;
    using Microsoft.EntityFrameworkCore;

    public class ManagerService(
        IRepository<Manager, Guid> managerRepository) : IManagerService
    {
        public async Task<bool> IsUserManager(string? userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return false;
            }

            bool result = await managerRepository
                .GetAllAttached()
                .AnyAsync(m => m.UserId.ToString().ToLower() == userId);

            return result;
        }
    }
}
