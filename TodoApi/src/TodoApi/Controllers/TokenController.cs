﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly TokenAuthOptions _tokenOptions;

        public TokenController(TokenAuthOptions tokenOptions)
        {
            _tokenOptions = tokenOptions;
        }

        /// <summary>
        /// Check if currently authenticated. Will throw an exception of some sort which shoudl be caught by a general
        /// exception handler and returned to the user as a 401, if not authenticated. Will return a fresh token if
        /// the user is authenticated, which will reset the expiry.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Token")]
        public dynamic Get()
        {
            /* 
            ******* WARNING WARNING WARNING ****** 
            ******* WARNING WARNING WARNING ****** 
            ******* WARNING WARNING WARNING ****** 
            THIS METHOD SHOULD BE REMOVED IN PRODUCTION USE-CASES - IT ALLOWS A USER WITH 
            A VALID TOKEN TO REMAIN LOGGED IN FOREVER, WITH NO WAY OF EVER EXPIRING THEIR
            RIGHT TO USE THE APPLICATION.
            Refresh Tokens (see https://auth0.com/docs/refresh-token) should be used to 
            retrieve new tokens. 
            ******* WARNING WARNING WARNING ****** 
            ******* WARNING WARNING WARNING ****** 
            ******* WARNING WARNING WARNING ****** 
            */

            bool authenticated = false;
            string user = null;
            int entityId = -1;
            string token = null;
            DateTime? tokenExpires = default(DateTime?);

            var currentUser = HttpContext.User;
            if (currentUser != null)
            {
                authenticated = currentUser.Identity.IsAuthenticated;
                if (authenticated)
                {
                    user = currentUser.Identity.Name;
                    foreach (Claim c in currentUser.Claims) if (c.Type == "EntityID") entityId = Convert.ToInt32(c.Value);
                    tokenExpires = DateTime.UtcNow.AddHours(2);
                    token = GetToken(currentUser.Identity.Name, tokenExpires);
                }
            }
            return new
            {
                authenticated = authenticated,
                user = user,
                entityId = entityId,
                token = token,
                tokenExpires = tokenExpires,
                claims = User.Claims.Select(c => new {c.Type, c.Value})
            };
        }

        public class AuthRequest
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        /// <summary>
        /// Request a new token for a given username/password pair.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public dynamic Post([FromBody] AuthRequest req)
        {
            // Obviously, at this point you need to validate the username and password against whatever system you wish.
            if ((req.username == "TEST" && req.password == "TEST") || (req.username == "TEST2" && req.password == "TEST"))
            {
                DateTime? expires = DateTime.UtcNow.AddHours(2);
                var token = GetToken(req.username, expires);
                return new { authenticated = true, entityId = 1, token = token, tokenExpires = expires };
            }
            return new { authenticated = false };
        }

        private string GetToken(string user, DateTime? expires)
        {
            var handler = new JwtSecurityTokenHandler();

            // Here, you should create or look up an identity for the user which is being authenticated.
            // For now, just creating a simple generic identity.
            ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(user, "TokenAuth"), new[] { new Claim("EntityID", "1", ClaimValueTypes.Integer) });

            var securityToken = handler.CreateJwtSecurityToken(
                issuer: _tokenOptions.Issuer,
                audience: _tokenOptions.Audience,
                signingCredentials: _tokenOptions.SigningCredentials,
                subject: identity,
                expires: expires
                );

            return handler.WriteToken(securityToken);
        }
    }
}