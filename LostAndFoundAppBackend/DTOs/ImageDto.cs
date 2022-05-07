using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFoundAppBackend.DTOs
{
    public class ImageDto
    {
        public int imageId { get; set; }
        public string title { get; set; }
        public string data { get; set; }
        public int itemId { get; set; }
    }
}
