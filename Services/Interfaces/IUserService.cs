namespace TestApp.Services.Interfaces
{
    public interface IUserService
    {
        bool IsUserInRole(string roleName);

        int GetCurrentUserId();
    }
}