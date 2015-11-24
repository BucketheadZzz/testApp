using System;
using System.Threading;
using System.Web.Security;
using WebMatrix.WebData;

namespace TestApp.Services
{
    public class UserService: IUserService
    {
        private static SimpleDBInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;
        public UserService()
        {
            if (!_isInitialized)
            {
                LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
            }
        }

 
        public bool IsUserInRole(string roleName)
        {
            return Roles.IsUserInRole(roleName);
        }

        public int GetCurrentUserId()
        {
            return WebSecurity.CurrentUserId;
        }

        private class SimpleDBInitializer
        {
            public SimpleDBInitializer()
            {
                try
                {
                    WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }

    }
}