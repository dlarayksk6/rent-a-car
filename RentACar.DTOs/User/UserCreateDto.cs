using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.DTOs.User
{
    public class UserCreateDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public List<int> SelectedPermissions { get; set; } = new();
    }
}