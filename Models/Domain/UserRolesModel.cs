using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace TestApp.Models.Domain
{

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