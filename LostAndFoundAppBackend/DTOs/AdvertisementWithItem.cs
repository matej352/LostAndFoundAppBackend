using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFoundAppBackend.DTOs
{
    public class AdvertisementWithItem
    {
        public int status { get; set; }
        public int accountId { get; set; }
        public int advertisementId { get; set; }
        public DateTime creationDate { get; set; }
        public DateTime expirationDate { get; set; }
        public ItemDto item { get; set; }
        public int found { get; set; }
        public int lost { get; set; }
    }
}
