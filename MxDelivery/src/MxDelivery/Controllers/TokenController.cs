using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using MxDelivery.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MxDelivery.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly TokenAuthOptions _tokenAuthOptions;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenController(TokenAuthOptions tokenAuthOptions, UserManager<ApplicationUser> userManager)
        {
            _tokenAuthOptions = tokenAuthOptions;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        //[Authorize(ActiveAuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Get()
        {
            string token = null;
            ClaimsPrincipal currentUser = HttpContext.User;
            if (currentUser != null)
            {
                var authenticated = currentUser.Identity.IsAuthenticated;
                if (authenticated)
                {
                    DateTime? tokenExpires = DateTime.UtcNow.AddHours(2);
                    token = GetToken(currentUser, tokenExpires);
                }
            }

            return Ok(new {token});
        }

        [HttpGet("test")]
        [Authorize(ActiveAuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Test()
        {
            return Ok(new
            {
                success = true,
                claims = User.Claims.Select(x => new {type = x.Type, value = x.Value})
            });
        }

        private string GetToken(ClaimsPrincipal user, DateTime? expires)
        {
            var handler = new JwtSecurityTokenHandler();

            // Here, you should create or look up an identity for the user which is being authenticated.
            // For now, just creating a simple generic identity.
            ClaimsIdentity identity = user.Identity as ClaimsIdentity;

            var securityToken = handler.CreateJwtSecurityToken(
                issuer: _tokenAuthOptions.Issuer,
                audience: _tokenAuthOptions.Audience,
                signingCredentials: _tokenAuthOptions.SigningCredentials,
                subject: identity,
                expires: expires
                );

            return handler.WriteToken(securityToken);
        }
    }
}
