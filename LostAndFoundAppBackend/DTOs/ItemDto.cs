using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFoundAppBackend.DTOs
{
    public class ItemDto
    {

        public int itemId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string pictureUrl { get; set; }
        public DateTime findingDate { get; set; }
        public DateTime lossDate { get; set; }
        public int AdvertisementId { get; set; }

    }
}
