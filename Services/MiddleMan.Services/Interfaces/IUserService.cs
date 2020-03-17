using MiddleMan.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiddleMan.Services.Interfaces
{
    public interface IUserService
    {
        Task<string> GetUsernameByIdAsync(string creatorId);

        Task<ApplicationUser> GetUserByIdAsync(string id);
    }
}
