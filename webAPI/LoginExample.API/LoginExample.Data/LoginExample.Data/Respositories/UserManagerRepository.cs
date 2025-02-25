using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoginExample.Data.Interfaces;
using LoginExample.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginExample.Data.Respositories
{
    public class UserManagerRepository : GenericRepository<AppUser>, IUserManagerRepository
    {
        IAppIdentityDbContext _context;
        public UserManagerRepository(IUnitOfWork<IAppIdentityDbContext> unitOfWork) : base(unitOfWork) { 
            _uow = unitOfWork;
            _context = unitOfWork.Context;
        }

        public async Task<AppUser?> FindByEmail(string email)
        {
            return await _context.AppUsers.Where(x => x.Email == email).FirstOrDefaultAsync();
        }
    }
}
