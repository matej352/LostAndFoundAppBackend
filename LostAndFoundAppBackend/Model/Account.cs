﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace EF.Model
{
    public partial class Account
    {
        public Account()
        {
            Advertisement = new HashSet<Advertisement>();
            Message = new HashSet<Message>();
        }

        public int AccountId { get; set; }
        public string Username { get; set; }
        public int PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Role { get; set; }

        public virtual ICollection<Advertisement> Advertisement { get; set; }
        public virtual ICollection<Message> Message { get; set; }
    }
}