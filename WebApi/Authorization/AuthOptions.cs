using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Authorization
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer";
        public const string AUDIENCE = "MyAuthClient"; 
        const string KEY = "secretkey_secretkey_secretkey";
        public const int LIFETIME = 2; //в часах
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
