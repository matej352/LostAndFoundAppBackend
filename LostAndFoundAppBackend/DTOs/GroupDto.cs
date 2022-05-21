using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFoundAppBackend.DTOs
{
    public class GroupDto
    {
        public string chatWith { get; set; }

        public MessageFromDbDto lastMessage { get; set; }


    }
}
