namespace AuthorizationService.Business.Models
{
    public enum Role
    {
        Undefined = 0,
        Base = 1,
        ManageUsers = 2,
        ManageTasks = 3,
        ManageNodes = 4,
        ManageSessions = 5
    }
}
