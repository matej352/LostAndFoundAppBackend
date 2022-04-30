using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFoundAppBackend.DTOs
{
    public class RegisterDto
    {

        public string Username { get; set; }
        public int PhoneNumber { get; set; }
        public byte[] Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] PasswordHashSalt { get; set; }

    }
}
