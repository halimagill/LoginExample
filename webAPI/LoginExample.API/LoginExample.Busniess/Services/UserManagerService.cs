﻿using System;
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
using Microsoft.EntityFrameworkCore;

namespace LoginExample.BSN.Services
{
    public class UserManagerService : IUserManagerService
    {
        private UserManager<AppUser> _userManager;
        private IUnitOfWork<IAppIdentityDbContext> _uow;


        public UserManagerService(UserManager<AppUser> usrMgr, IUnitOfWork<IAppIdentityDbContext> uow)
        {
            //   _logger = logger;
            _uow = uow;
            _userManager = usrMgr;
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
                EmailConfirmed = true,
                PhoneNumber = user.PhoneNo,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            //Add email check on front end
            AppUser? findUser = await _userManager.FindByEmailAsync(user.Email);

            if(findUser is null)
            {
                IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);

                return result;
            }
            
            return IdentityResult.Failed(new IdentityError
            { Code = "User_found", Description = "Email already exists." });
        }

        public async Task<IEnumerable<User?>> GetUsers()
        {
            return _userManager.Users.ToDTO();
        }

        public async Task<AppUser?> FindUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<AppUser?> FindUserById(Guid id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }

        public async Task<User?> UpdateUser(User user)
        {            
            AppUser? findUser = await FindUserById(user.Id);

            if (findUser != null)
            {
                findUser.Email = user.Email;
                findUser.FirstName = user.FirstName;
                findUser.LastName = user.LastName;
                findUser.UserName = user.UserName;
                findUser.PhoneNumber = user.PhoneNo;

                var updatedUser = await _userManager.UpdateAsync(findUser);
            }

            return findUser?.ToDTO();
        }

        public async Task<bool> VerifyPassword(AppUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
     }
}
