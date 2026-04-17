using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.DTOs.User
{
    public class UserListDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; } = new();
        public List<string> Permissions { get; set; } = new();
    }
}