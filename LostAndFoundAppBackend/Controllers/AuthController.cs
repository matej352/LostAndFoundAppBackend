using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EF.Model;
using LostAndFoundAppBackend.DTOs;
using LostAndFoundAppBackend.Repository;
using Microsoft.AspNetCore.Http;

namespace LostAndFoundAppBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LostandfoundappdbContext context;
        private readonly IAccountRepository repository;

        private IConfiguration configuration;



        public AuthController(LostandfoundappdbContext context, IConfiguration configuration, IAccountRepository repository)
        {
            this.context = context;
            this.repository = repository;
            this.configuration = configuration;

        }

        [HttpPost("register")]
        public async Task<ActionResult<AccountDto>> Register(CreateAccountDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            //provjera je li username unique (front i back)
            var usernameExists = context.Account.FirstOrDefault(u => u.Username == request.Username);

            if (usernameExists != null) 
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Username {usernameExists.Username} already taken.");
            }

            RegisterDto acc = new RegisterDto
            {
                Username = request.Username,
                PhoneNumber = request.PhoneNumber,
                Password = passwordHash,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PasswordHashSalt = passwordSalt

            };


            int acc_id = await repository.save(acc);
            Account accountInRepo = (await repository.findById(acc_id)).Value;


            return Ok(accountInRepo.AsAccountDto());
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto request)
        {

          
            var user = context.Account.FirstOrDefault(u => u.Username == request.Username);
            if (user == null)
            {
                return BadRequest("Incorrect username or password!");
            }

            if (!VerifyPasswordHash(request.Password, user.Password, user.PasswordHashSalt))
            {
                return BadRequest("Incorrect username or password!");
            }

            //GENERATE JWT

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:TokenKey").Value));

            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);


            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role == (int)Role.User ? Role.User.ToString() : Role.Admin.ToString())

            };                                                  
                                                                

            var tokenOptions = new JwtSecurityToken(
                    issuer: configuration.GetSection("AppSettings:Issuer").Value,  
                    audience: configuration.GetSection("AppSettings:Audience").Value, 
                                                                                     
                    claims: claims,  
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signingCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions); 

            return Ok(new { token = tokenString });

        }





        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA256())  
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA256(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }




    }
}
