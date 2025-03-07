using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoginExample.Data.Models;

namespace LoginExample.Data
{
    public static class DTOMapper
    {
        public static User ToDTO(this AppUser searchEntity)
        {
            if (searchEntity == null) return null;

            return new User
            {
                UserName = searchEntity.UserName ?? string.Empty,
                Email = searchEntity.Email ?? string.Empty,
                PhoneNo = searchEntity.PhoneNumber ?? string.Empty,
                FirstName = searchEntity.FirstName,
                LastName = searchEntity.LastName,
                IsActive = searchEntity.IsActive
            };
        }

        public static IEnumerable<User> ToDTO(this IEnumerable<AppUser> searchResults)
        {
            if (searchResults == null) return null;

            var dto = new List<User>();

            foreach (var result in searchResults)
            {
                dto.Add(result.ToDTO());
            }

            return dto;
        }
    }
}
