using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services.Data.Contracts
{
    public interface IManagerService
    {
        Task<bool> IsUserManager(string? userId);
    }
}
