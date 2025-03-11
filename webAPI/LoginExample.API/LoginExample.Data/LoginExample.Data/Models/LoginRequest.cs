using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginExample.Data.Models
{
    public class LoginRequest
    {
        [Required]
        public string email {  get; set; }
        [Required]
        public string password { get; set; }
    }
}
