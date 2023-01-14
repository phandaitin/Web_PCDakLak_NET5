using ApiApp.Data;
using ApiApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _config;

        public UserController(MyDbContext context , IConfiguration config) {
            _context = context;
            _config = config;
        }
        [HttpPost("register")]  
        public IActionResult Register(RegisterVM model)
        {
            var user = new User()
            {
                UserName = model.UserName,
                FullName = model.FullName,
                Email = model.Email,
                Password = model.Password,
                Phone = model.Phone,
                Dob = model.Dob
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok();
        }

         
        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email , user.Email),
                new Claim("UserName" , user.UserName),
                new Claim(ClaimTypes.GivenName , user.FullName)
              //  new Claim(ClaimTypes.Role , string.Join(":",  roles ))
            };
            var _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:SecretKey"]));
            var _signingCredentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: _signingCredentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpPost("login")] // cach 2
        public IActionResult Authenticate(LoginVM model)
        {
            var user = _context.Users.SingleOrDefault(x => x.UserName == model.UserName && x.Password == model.Password);
            if (user == null)
            {
                return Ok(new  ResponseApi
                {
                    Success = false,
                    Message = "Invalid UserName or Password",
                    Token = null
                });
            }

            // cap token

            return Ok(new ResponseApi
            {
                Success = true,
                Message = "Authenticate Success !!!",
                Token = GenerateToken(user)
            });
        }
        //[HttpPost("login")] cach 3
        //private string GenerateToken(NguoiDung nguoiDung)
        //{
        //    var jwtTokenHandler = new JwtSecurityTokenHandler();

        //    var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

        //    var tokenDescription = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new[] {
        //        new Claim(ClaimTypes.Name, nguoiDung.HoTen),
        //        new Claim(ClaimTypes.Email, nguoiDung.Email),
        //        new Claim("UserName", nguoiDung.UserName),
        //        new Claim("Id", nguoiDung.Id.ToString()),

        //        //roles

        //        new Claim("TokenId", Guid.NewGuid().ToString())
        //    }),
        //        Expires = DateTime.UtcNow.AddMinutes(1),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
        //    };

        //    var token = jwtTokenHandler.CreateToken(tokenDescription);

        //    return jwtTokenHandler.WriteToken(token);
        //}



    }



}
