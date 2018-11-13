namespace AuthorizationService.Repositories.DAL
{
    public interface IAuthDBContextFactory
    {
        IAuthDBContext CreateDBContext();
    }
}
