using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoginExample.Data.Models;

namespace LoginExample.Data.Interfaces
{
    public interface IUserManagerRepository
    {
        Task<AppUser?> FindByEmail(string email);
    }
}
