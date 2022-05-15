using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFoundAppBackend.DTOs
{
    public class MessageDto
    {
         public string content { get; set; }
         public int recieverId { get; set; }
         public string From { get; set; }
    }
}
