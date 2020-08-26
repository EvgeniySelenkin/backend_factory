using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class User//класс для авторизации пользователя
    {
        public string login { get; set; }
        public string password { get; set; }
    }
}
