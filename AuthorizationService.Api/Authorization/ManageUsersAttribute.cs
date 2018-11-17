using System.Collections.Generic;
using AuthorizationService.Business;
using AuthorizationService.Business.Models;
using Microsoft.Extensions.Logging;

namespace AuthorizationService.Api.Authorization
{
    public class ManageUsersAttribute : BaseAuthorizationAttribute
    {
        public ManageUsersAttribute() : base(typeof(ManageUsersAuthorizationFilter))
        {
        }
    }

    public class ManageUsersAuthorizationFilter : BaseAuthorizationFilter
    {
        public ManageUsersAuthorizationFilter(IAuthService authService) : base(authService)
        {
            Roles = new List<Role> { Role.ManageUsers };
        }
    }
}