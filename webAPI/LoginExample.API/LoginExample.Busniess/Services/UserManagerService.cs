using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using LoginExample.BSN.Interfaces;
using LoginExample.Data;
using LoginExample.Data.Interfaces;
using LoginExample.Data.Models;
using LoginExample.Data.Respositories;
using Microsoft.AspNetCore.Identity;

namespace LoginExample.BSN.Services
{
    public class UserManagerService : IUserManagerService
    {
        private UserManager<AppUser> _userManager;
        private IUserManagerRepository _userManagerRepo;
        private IUnitOfWork<IAppIdentityDbContext> _uow;


        public UserManagerService(UserManager<AppUser> usrMgr, IUnitOfWork<IAppIdentityDbContext> uow)
        {
            //   _logger = logger;
            _uow = uow;
            _userManager = usrMgr;  
            _userManagerRepo = _uow.Repository<UserManagerRepository>();
        }

        //TO DO CREATE CUSTOM RESULT
        public async Task<IdentityResult> CreateUser(User user)
        {            
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            AppUser appUser = new AppUser
            {
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            //Add email check on front end
            AppUser? findUser = await _userManagerRepo.FindByEmail(user.Email);

            if(findUser is null)
            {
                IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);

                return result;
            }

            return IdentityResult.Failed(new IdentityError
            { Code = "User_found", Description = "Email already exists." });
        }

        public async Task<AppUser?> FindUserByEmail(string email)
        {
            return await _userManagerRepo.FindByEmail(email);
        }
     }
}
