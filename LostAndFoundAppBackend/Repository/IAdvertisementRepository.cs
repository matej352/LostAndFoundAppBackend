using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using EF.Model;
using LostAndFoundAppBackend.DTOs;

namespace LostAndFoundAppBackend.Repository
{
    public interface IAdvertisementRepository
    {
        public Task<ActionResult<Account>> findAccById(int id);

        public Task<ActionResult<int>> findIdByUsername(string username);

        public Task<ActionResult<Advertisement>> findAdvById(int id);

        public Task<int> save(CreateAdvertisementDto adv);
        public Task<List<AdvertisementWithItem>> GetAllActive(QueryOptionsDto query);
        public Task<List<AdvertisementWithItem>> GetAll(QueryOptionsDto query);

        public Task<List<AdvertisementWithItem>> GetAllActive();

        public Task<List<AdvertisementWithItem>> GetAllActive(int categoryId);
        public Task<List<AdvertisementWithItem>> GetAll(int accountId);

        public Task<List<AdvertisementWithItem>> GetAllInactive(int accountId);


        public Task<List<AdvertisementWithItem>> GetAllActive(int categoryId, QueryOptionsDto query);

        public Task<AdvertisementWithItem> GetAdvertisementWithItem(int id);
        public Task UpdateStatus(int advertisementId);


        public Task<int> GetAllCount();
        public Task<Advertisement> UpdateExpirationDate(int advertisementId);

        public Task Delete(int advId);
    }
}
