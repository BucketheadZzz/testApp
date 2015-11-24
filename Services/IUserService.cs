namespace TestApp.Services
{
    public interface IUserService
    {
        bool IsUserInRole(string roleName);

        int GetCurrentUserId();
    }
}