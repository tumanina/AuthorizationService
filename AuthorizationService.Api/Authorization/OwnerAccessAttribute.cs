using System.Collections.Generic;
using AuthorizationService.Business;
using AuthorizationService.Business.Models;

namespace AuthorizationService.Api.Authorization
{
    public class OwnerAccessAttribute : BaseAuthorizationAttribute
    {
        public OwnerAccessAttribute() : base(typeof(OwnerAccessAuthorizationFilter))
        {
        }
    }

    public class OwnerAccessAuthorizationFilter : BaseAuthorizationFilter
    {
        public OwnerAccessAuthorizationFilter(IAuthService authService) : base(authService)
        {
            Roles = new List<Role>();
        }
    }
}