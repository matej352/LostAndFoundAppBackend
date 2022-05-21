using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFoundAppBackend.DTOs
{
    public class MessageFromDbDto
    {

        public int messageId { get; set; }
        public string content { get; set; }
        public int recieverId { get; set; }
        public int accountId { get; set; }
        public DateTime sendDateTime { get; set; }
        public DateTime? ReadDateTime { get; set; }
        public int isRead { get; set; }

    }
}
