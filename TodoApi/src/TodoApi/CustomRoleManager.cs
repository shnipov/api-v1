using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace TodoApi
{
    public class CustomRoleManager : RoleManager<Role>
    {
        public CustomRoleManager(
            IRoleStore<Role> store, 
            IEnumerable<IRoleValidator<Role>> roleValidators, 
            ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, 
            ILogger<CustomRoleManager> logger, 
            Microsoft.AspNetCore.Http.IHttpContextAccessor contextAccessor) 
            : base(
                store, 
                roleValidators, 
                keyNormalizer, 
                errors, 
                logger, 
                contextAccessor)
        {
        }
    }
}