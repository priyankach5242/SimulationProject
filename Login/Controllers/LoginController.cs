using System;
//using System.Collections.Generic;
//using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
//using System.Security.Cryptography.X509Certificates;
using System.Text;
using Login.Models;
using Login.Models.ViewModel;
using Login.Repository;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Login.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(LoginController));
        private IConfiguration _config;
        private ILoginRepo loginRepo;
        public LoginController(IConfiguration config, ILoginRepo login)
        {
            this._config = config;
            this.loginRepo = login;
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            _log4net.Info("Authentication initiated");
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            var RightUser = await loginRepo.GetAccount(model);
            if (RightUser == null)
            {
                //ModelState.AddModelError("", "Invalid Username or Password");
                return NotFound();
            }
            else
            {
                _log4net.Info("Login credential matched");
                return Ok(new
                {
                    token = GenerateJWT(RightUser)
                });
            }
        }

        [HttpGet]
        [Route("GetMessage")]
        public IActionResult GetMessage()
        {
            return Content("Hello");
        }
        private string GenerateJWT(LoginModel userLogin)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(180),
              signingCredentials: credentials);
            _log4net.Info("Json web token generated");


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

