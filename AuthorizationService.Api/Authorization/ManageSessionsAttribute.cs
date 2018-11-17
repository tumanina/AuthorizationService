using System.Collections.Generic;
using AuthorizationService.Business;
using AuthorizationService.Business.Models;

namespace AuthorizationService.Api.Authorization
{
    public class ManageSessionsAttribute : BaseAuthorizationAttribute
    {
        public ManageSessionsAttribute() : base(typeof(ManageSessionsAuthorizationFilter))
        {
        }
    }

    public class ManageSessionsAuthorizationFilter : BaseAuthorizationFilter
    {
        public ManageSessionsAuthorizationFilter(IAuthService authService) : base(authService)
        {
            Roles = new List<Role> { Role.ManageSessions };
        }
    }
}