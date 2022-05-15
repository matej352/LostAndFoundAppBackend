using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFoundAppBackend.DTOs
{
    public class CreateItemDto
    {

        public string title { get; set; }
        public string description { get; set; }
        public DateTime? findingDate { get; set; }
        public DateTime? lossDate { get; set; }
        public int AdvertisementId { get; set; }
        public int categoryId { get; set; }

    }
}
