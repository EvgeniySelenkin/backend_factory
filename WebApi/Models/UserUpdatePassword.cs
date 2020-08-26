using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class UserUpdatePassword
    {
        public string login { get; set; }
        public string currentPassword { get; set; }
        public string newPassword { get; set; }
    }
}
