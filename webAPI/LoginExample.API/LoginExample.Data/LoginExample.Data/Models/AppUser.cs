using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LoginExample.Data.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; } = true;

        // public record AppUser(Guid Id, string Name, string Email, string Password, string[] Roles);

        /*
         * 
         --------------
          userId : 0,
            firstName : val.firstName,
            middleName : "N/A",
            lastName : val.lastName,
            emailId : val.emailId,
            password : val.password,
            mobileNo : "N/A",
            altMobileNo : "N/A"
         --------------
         Id	It contains the Unique Id of the user.
    UserName	It contains the user’s username.
    Claims	This property returns all the claims for the user.
    Email	It contains the email for the user.
    PasswordHash	It contains the hash form of the user password.
    Roles	It returns all the roles for the user.
    PhoneNumber	It returns the phone number for the user.
    SecurityStamp
         */
    }
}
