using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFoundAppBackend.DTOs
{
    public class CreateAdvertisementDto
    {
        public int accountId { get; set; }
        public int lost { get; set; }
        public int found { get; set; }
    }
}
