using Microsoft.EntityFrameworkCore;

namespace AuthorizationService.Repositories.DAL
{
    public class AuthDBContextFactory : IAuthDBContextFactory
    {
        private readonly DbContextOptionsBuilder<AuthDBContext> _options;

        public AuthDBContextFactory(DbContextOptionsBuilder<AuthDBContext> options)
        {
            _options = options;
        }

        public IAuthDBContext CreateDBContext()
        {
            return new AuthDBContext(_options.Options);
        }
    }
}
