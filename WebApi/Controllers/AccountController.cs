using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApi.Models;
using WebApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly DBContext context;
        public AccountController(DBContext context)
        {
            this.context = context;
        }

        [Authorize]
        [HttpGet("api/user/current")]
        public IActionResult GetCurrentPerson()
        {
            return Ok($"Авторизирован аккаунт с логином :{User.Identity.Name}");
        }

        [HttpPost("api/user/auth")]
        public IActionResult Token(User user)
        {
            var md5 = new MD5Hash(user.password);
            var hashPassword = md5.GetMd5Hash();
            var identity = GetIdentity(user.login, hashPassword);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromHours(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Ok(response);
        }

        private ClaimsIdentity GetIdentity(string username, string hashPassword)
        {
            Person person = context.Person.FirstOrDefault(x => x.Login == username && x.hashPassword == hashPassword);
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }

        [Authorize]
        [HttpPost("api/user/password/update")]
        public IActionResult UpdatePassword(UserUpdatePassword user)
        {
            var md5 = new MD5Hash(user.currentPassword);
            var hashPassword = md5.GetMd5Hash();
            var identity = GetIdentity(user.login, hashPassword);
            if (identity ==null)
                return BadRequest(new { errorText = "Invalid username or password." });

            md5 = new MD5Hash(user.newPassword);
            var hashNewPassword = md5.GetMd5Hash();
            Person person = context.Person.FirstOrDefault(x => x.Login == user.login);
            person.hashPassword = hashNewPassword;
            context.Person.Update(person);
            context.SaveChangesAsync();
            return Ok("Пароль успешно изменен");
        }
    }
}
