using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EF.Model;
using LostAndFoundAppBackend.DTOs;

namespace LostAndFoundAppBackend.Repository
{
   
    public class AdvertisementRepository : IAdvertisementRepository
    {


        private readonly LostandfoundappdbContext context;

        public AdvertisementRepository(LostandfoundappdbContext context)
        {
            this.context = context;
        }


        public async Task<ActionResult<Account>> findAccById(int id)
        {
            var acc = await context.Account.FindAsync(id);

            return await Task.FromResult(acc);
        }

        public async Task<ActionResult<int>> findIdByUsername(string username)
        {
            var acc = context.Account.Where(a => a.Username == username).SingleOrDefault();

            return await Task.FromResult(acc.AccountId);
        }

        public async Task<ActionResult<Advertisement>> findAdvById(int id)
        {
            var adv = await context.Advertisement.FindAsync(id);

            return await Task.FromResult(adv);
        }

        public Task<AdvertisementWithItem> GetAdvertisementWithItem(int id)
        {
            var adWithItem = context.Advertisement.Where(a => a.AdvertisementId == id)
                                 .Join(
                                        context.Item,
                                         p => p.AdvertisementId,
                                         e => e.AdvertisementId,
                                         (p, e) => new { p, e }
                                       ).Join(
                                              context.Category,
                                              a => a.e.CategoryId,
                                              b => b.CategoryId,
                         (a, b) => new AdvertisementWithItem
                         {
                             status = a.p.Status,
                             accountId = a.p.AccountId,
                             advertisementId = a.p.AdvertisementId,
                             creationDate = a.p.PublishDate,
                             expirationDate = a.p.ExpirationDate,
                             found = (int)a.p.Found,
                             lost = (int)a.p.Lost,

                             item = new ItemDto
                             {
                                 itemId = a.e.ItemId,
                                 title = a.e.Title,
                                 description = a.e.Description,
                                 locationLng = (float?)a.e.LocationLng,
                                 locationLat = (float?)a.e.LocationLat,
                                 findingDate = (DateTime)a.e.FindingDate,
                                 lossDate = (DateTime)a.e.LossDate,
                                 AdvertisementId = a.e.AdvertisementId,
                                 category = new CategoryDto
                                 {
                                     categoryId = b.CategoryId,
                                     name = b.Name
                                 }
                             }
                         }
                    ).SingleOrDefault();


            return Task.FromResult(adWithItem);
        }


        public Task<List<AdvertisementWithItem>> GetAllActive()
        {
            var adsWithItems = context.Advertisement.Where(a => a.Status == 1 && DateTime.Compare(DateTime.UtcNow, a.ExpirationDate) < 0)
                                 .OrderByDescending(a => a.PublishDate)
                                 .Join(
                                        context.Item,
                                         p => p.AdvertisementId,
                                         e => e.AdvertisementId,
                                         (p, e) => new { p, e }
                                       ).Join(
                                              context.Category,
                                              a => a.e.CategoryId,
                                              b => b.CategoryId,
                         (a, b) => new AdvertisementWithItem
                         {
                             status = a.p.Status,
                             accountId = a.p.AccountId,
                             advertisementId = a.p.AdvertisementId,
                             creationDate = a.p.PublishDate,
                             expirationDate = a.p.ExpirationDate,
                             found = (int)a.p.Found,
                             lost = (int)a.p.Lost,

                             item = new ItemDto
                             {
                                 itemId = a.e.ItemId,
                                 title = a.e.Title,
                                 description = a.e.Description,
                                 locationLng = (float?)a.e.LocationLng,
                                 locationLat = (float?)a.e.LocationLat,
                                 findingDate = (DateTime)a.e.FindingDate,
                                 lossDate = (DateTime)a.e.LossDate,
                                 AdvertisementId = a.e.AdvertisementId,
                                 category = new CategoryDto
                                 {
                                     categoryId = b.CategoryId,
                                     name = b.Name
                                 }
                             }
                         }
                    ).ToList();

            findImageOfItem(adsWithItems);


            return Task.FromResult(adsWithItems);
        }

        public Task<List<AdvertisementWithItem>> GetAllActive(QueryOptionsDto query)
        {
            var adsWithItems = context.Advertisement.Where(a => a.Status == 1 && DateTime.Compare(DateTime.UtcNow, a.ExpirationDate) < 0)
                                 .OrderByDescending(a => a.PublishDate)
                                 .Skip(query.startIndex).Take(query.endIndex-query.startIndex)
                                 .Join(
                                        context.Item,
                                         p => p.AdvertisementId,
                                         e => e.AdvertisementId,
                                         (p, e) => new { p, e }
                                       ).Join(
                                              context.Category,
                                              a => a.e.CategoryId,
                                              b => b.CategoryId,
                         (a, b) => new AdvertisementWithItem
                         {
                            status = a.p.Status,
                            accountId = a.p.AccountId,
                            advertisementId = a.p.AdvertisementId,
                            creationDate = a.p.PublishDate,
                            expirationDate = a.p.ExpirationDate,
                            found = (int)a.p.Found,
                            lost = (int)a.p.Lost,

                            item = new ItemDto
                            {
                                itemId = a.e.ItemId,
                                title = a.e.Title,
                                description = a.e.Description,
                                locationLng = (float?)a.e.LocationLng,
                                locationLat = (float?)a.e.LocationLat,
                                findingDate = (DateTime)a.e.FindingDate,
                                lossDate = (DateTime)a.e.LossDate,
                                AdvertisementId = a.e.AdvertisementId,
                                category = new CategoryDto
                                {
                                    categoryId = b.CategoryId,
                                    name = b.Name
                                }
                            }
                        }
                    ).ToList();

            findImageOfItem(adsWithItems);


            return Task.FromResult(adsWithItems);
        }

        public Task<List<AdvertisementWithItem>> GetAll(int accountId)
        {
            var adsWithItems = context.Advertisement.Where(a => a.AccountId == accountId && DateTime.Compare(DateTime.UtcNow, a.ExpirationDate) < 0)
                                  .OrderByDescending(a => a.PublishDate)
                                 .Join(
                                        context.Item,
                                         p => p.AdvertisementId,
                                         e => e.AdvertisementId,
                                         (p, e) => new { p, e }
                                       ).Join(
                                              context.Category,
                                              a => a.e.CategoryId,
                                              b => b.CategoryId,
                         (a, b) => new AdvertisementWithItem
                         {
                             status = a.p.Status,
                             accountId = a.p.AccountId,
                             advertisementId = a.p.AdvertisementId,
                             creationDate = a.p.PublishDate,
                             expirationDate = a.p.ExpirationDate,
                             found = (int)a.p.Found,
                             lost = (int)a.p.Lost,

                             item = new ItemDto
                             {
                                 itemId = a.e.ItemId,
                                 title = a.e.Title,
                                 description = a.e.Description,
                                 locationLng = (float?)a.e.LocationLng,
                                 locationLat = (float?)a.e.LocationLat,
                                 findingDate = (DateTime)a.e.FindingDate,
                                 lossDate = (DateTime)a.e.LossDate,
                                 AdvertisementId = a.e.AdvertisementId,
                                 category = new CategoryDto
                                 {
                                     categoryId = b.CategoryId,
                                     name = b.Name
                                 }
                             }
                         }
                    ).ToList();

            findImageOfItem(adsWithItems);


            return Task.FromResult(adsWithItems);
        }

        public Task<List<AdvertisementWithItem>> GetAllInactive(int accountId)
        {
            var adsWithItems = context.Advertisement.Where(a => a.AccountId == accountId && DateTime.Compare(DateTime.UtcNow, a.ExpirationDate) > 0)
                                  .OrderByDescending(a => a.PublishDate)
                                 .Join(
                                        context.Item,
                                         p => p.AdvertisementId,
                                         e => e.AdvertisementId,
                                         (p, e) => new { p, e }
                                       ).Join(
                                              context.Category,
                                              a => a.e.CategoryId,
                                              b => b.CategoryId,
                         (a, b) => new AdvertisementWithItem
                         {
                             status = a.p.Status,
                             accountId = a.p.AccountId,
                             advertisementId = a.p.AdvertisementId,
                             creationDate = a.p.PublishDate,
                             expirationDate = a.p.ExpirationDate,
                             found = (int)a.p.Found,
                             lost = (int)a.p.Lost,

                             item = new ItemDto
                             {
                                 itemId = a.e.ItemId,
                                 title = a.e.Title,
                                 description = a.e.Description,
                                 locationLng = (float?)a.e.LocationLng,
                                 locationLat = (float?)a.e.LocationLat,
                                 findingDate = (DateTime)a.e.FindingDate,
                                 lossDate = (DateTime)a.e.LossDate,
                                 AdvertisementId = a.e.AdvertisementId,
                                 category = new CategoryDto
                                 {
                                     categoryId = b.CategoryId,
                                     name = b.Name
                                 }
                             }
                         }
                    ).ToList();

            findImageOfItem(adsWithItems);


            return Task.FromResult(adsWithItems);
        }

        private void findImageOfItem(List<AdvertisementWithItem> adsWithItems)
        {
            for (int i = 0; i < adsWithItems.Count; i++)
            {
                var item = adsWithItems[i].item;
                byte[] dataByte = new byte[Byte.MaxValue];
                dataByte = context.Image.Where(image => image.ItemId == item.itemId).Select(i => i.Data).FirstOrDefault();
                if (dataByte != null)
                {
                    string imageBase64Data = Convert.ToBase64String(dataByte);
                    adsWithItems[i].item.imageData = imageBase64Data;
                }
                else
                {
                    adsWithItems[i].item.imageData = null;
                }

            }
        }

        public Task<List<AdvertisementWithItem>> GetAllActive(int categoryId, QueryOptionsDto query)
        {

            var adsWithItems = context.Advertisement.Where(a => a.Status == 1 && DateTime.Compare(DateTime.UtcNow, a.ExpirationDate) < 0)
                                 .OrderByDescending(a => a.PublishDate)
                                 .Skip(query.startIndex).Take(query.endIndex - query.startIndex)
                                 .Join(
                                        context.Item,
                                         p => p.AdvertisementId,
                                         e => e.AdvertisementId,
                                         (p, e) => new { p, e }
                                       ).Join(
                                              context.Category.Where(c => c.CategoryId == categoryId),
                                              a => a.e.CategoryId,
                                              b => b.CategoryId,
                         (a, b) => new AdvertisementWithItem
                         {
                             status = a.p.Status,
                             accountId = a.p.AccountId,
                             advertisementId = a.p.AdvertisementId,
                             creationDate = a.p.PublishDate,
                             expirationDate = a.p.ExpirationDate,
                             found = (int)a.p.Found,
                             lost = (int)a.p.Lost,

                             item = new ItemDto
                             {
                                 itemId = a.e.ItemId,
                                 title = a.e.Title,
                                 description = a.e.Description,
                                 locationLng = (float?)a.e.LocationLng,
                                 locationLat = (float?)a.e.LocationLat,
                                 findingDate = (DateTime)a.e.FindingDate,
                                 lossDate = (DateTime)a.e.LossDate,
                                 AdvertisementId = a.e.AdvertisementId,
                                 category = new CategoryDto
                                 {
                                     categoryId = b.CategoryId,
                                     name = b.Name
                                 }
                             }
                         }
                    ).ToList();

            if (adsWithItems != null)
            {
                findImageOfItem(adsWithItems);
            }

            return Task.FromResult(adsWithItems);
        }


        public Task<List<AdvertisementWithItem>> GetAllActive(int categoryId)
        {
        
            var adsWithItems = context.Advertisement.Where(a => a.Status == 1 && DateTime.Compare(DateTime.UtcNow, a.ExpirationDate) < 0)
                                 .OrderByDescending(a => a.PublishDate)
                                 .Join(
                                        context.Item,
                                         p => p.AdvertisementId,
                                         e => e.AdvertisementId,
                                         (p, e) => new { p, e }
                                       ).Join(
                                              context.Category.Where(c => c.CategoryId == categoryId),
                                              a => a.e.CategoryId,
                                              b => b.CategoryId,
                         (a, b) => new AdvertisementWithItem
                         {
                             status = a.p.Status,
                             accountId = a.p.AccountId,
                             advertisementId = a.p.AdvertisementId,
                             creationDate = a.p.PublishDate,
                             expirationDate = a.p.ExpirationDate,
                             found = (int)a.p.Found,
                             lost = (int)a.p.Lost,

                             item = new ItemDto
                             {
                                 itemId = a.e.ItemId,
                                 title = a.e.Title,
                                 description = a.e.Description,
                                 locationLng = (float?)a.e.LocationLng,
                                 locationLat = (float?)a.e.LocationLat,
                                 findingDate = (DateTime)a.e.FindingDate,
                                 lossDate = (DateTime)a.e.LossDate,
                                 AdvertisementId = a.e.AdvertisementId,
                                 category = new CategoryDto
                                 {
                                     categoryId = b.CategoryId,
                                     name = b.Name
                                 }
                             }
                         }
                    ).ToList();
            if (adsWithItems != null) {
                findImageOfItem(adsWithItems);
            }

            return Task.FromResult(adsWithItems);
        }

        public async Task<int> save(CreateAdvertisementDto adv)
        {
            var id = await findIdByUsername(adv.username);

            Advertisement savedAdv = new Advertisement
            {
                Status = 1,
                PublishDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddDays(31),
                AccountId = id.Value,
                Found = adv.found,
                Lost = adv.lost,
            };

            context.Add(savedAdv);
            await context.SaveChangesAsync();
            return await Task.FromResult(savedAdv.AdvertisementId);
        }

        public async Task<Advertisement> UpdateExpirationDate(int advertisementId)
        {
            var adv = context.Advertisement.Where(a => a.AdvertisementId == advertisementId).SingleOrDefault();

           
            adv.ExpirationDate = adv.ExpirationDate.AddDays(31);
            adv.PublishDate = DateTime.UtcNow;

            await context.SaveChangesAsync();
            return await Task.FromResult(adv);
        }

        public async Task UpdateStatus(int advertisementId)
        {
            var adv = await context.Advertisement.FindAsync(advertisementId);
            if (adv.Status == 1)
            {
                adv.Status = 0;
            }
            else 
            {
                adv.Status = 1;
            }

            await context.SaveChangesAsync();
        }


    }
}
