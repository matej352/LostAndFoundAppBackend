using EF.Model;
using LostAndFoundAppBackend.DTOs;

namespace LostAndFoundAppBackend.Controllers
{
    public static class Extensions
    {
        public static AccountDto AsAccountDto(this Account acc)
        {
            return new AccountDto
            {
                AccountId = acc.AccountId,
                Username = acc.Username,
                PhoneNumber = acc.PhoneNumber,
                Password = acc.Password,
                Email = acc.Email,
                FirstName = acc.FirstName,
                LastName = acc.LastName,
                Role = acc.Role,
                Active = (int)acc.Active
            };

        }

        public static AdvertisementDto AsAdvertisementDto(this Advertisement adv)
        {
            return new AdvertisementDto
            {
                advertisementId = adv.AdvertisementId,
                status = adv.Status,
                accountId = adv.AccountId,
                creationDate = adv.PublishDate,
            };
        }


      
        public static ItemDto AsItemDto(this Item item)
        {
            return new ItemDto
            {
                 itemId = item.ItemId,
                 title = item.Title,
                 description = item.Description,
                 pictureUrl = item.PictureUrl,
                 findingDate = (System.DateTime)item.FindingDate,
                 lossDate = (System.DateTime)item.LossDate,
                 AdvertisementId =item.AdvertisementId
             };
        }



    }
}
