using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoginExample.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace LoginExample.BSN.Interfaces
{
    public  interface IUserManagerService
    {
        Task<IdentityResult> CreateUser(User user);
        Task<AppUser?> FindUserByEmail(string email);
    }
}
