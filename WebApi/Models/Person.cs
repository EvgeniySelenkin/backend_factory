using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string hashPassword { get; set; }
        public string Role { get; set; }
    }
}
