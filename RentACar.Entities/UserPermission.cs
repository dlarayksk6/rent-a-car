using Microsoft.AspNetCore.Identity; 

namespace RentACar.Entities
{
    public class UserPermission
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PermissionId { get; set; }

        public IdentityUser User { get; set; }
        public Permission Permission { get; set; }
    }
}

