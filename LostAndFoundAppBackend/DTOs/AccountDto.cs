﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFoundAppBackend.DTOs
{
    public class AccountDto
    {
        public int AccountId { get; set; }
        public string Username { get; set; }
        public int PhoneNumber { get; set; }
        public byte[] Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Role { get; set; }
        public int Active { get; set; }
    }
}
