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

        public async Task<ActionResult<Advertisement>> findAdvById(int id)
        {
            var adv = await context.Advertisement.FindAsync(id);

            return await Task.FromResult(adv);
        }
       
        public Task<List<AdvertisementWithItem>> GetAllActive()
        {



            var adsWithItems = context.Advertisement
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
                                pictureUrl = a.e.PictureUrl,
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

            
            return Task.FromResult(adsWithItems);
        }

        public async Task<int> save(CreateAdvertisementDto adv)
        {
            Advertisement savedAdv = new Advertisement
            {
                Status = 1,
                PublishDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddDays(30),
                AccountId = adv.accountId,
                Found = adv.found,
                Lost = adv.lost,
            };

            context.Add(savedAdv);
            await context.SaveChangesAsync();
            return await Task.FromResult(savedAdv.AdvertisementId);
        }

        
    }
}
