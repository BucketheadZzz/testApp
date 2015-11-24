using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace TestApp.Models
{
    public class UserRolesContext: DbContext
    {
        public UserRolesContext()
            : base("DefaultConnection")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserRoles>().HasKey(x => x.RoleId);
            modelBuilder.Entity<UserRolesMapping>().HasKey(x => x.RoleId);
        }

        public DbSet<UserRoles> UserRoles { get; set; }

        public DbSet<UserRolesMapping> UserRolesMappings { get; set; }
    }

    [Table("webpages_Roles")]
    public class UserRoles
    {
 
        public int RoleId { get; set; }

        public string RoleName { get; set; }
    }


    [Table("webpages_UsersInRoles")]
    public class UserRolesMapping
    {
        public int RoleId { get; set; }

        public int UserId { get; set; }
    }

}