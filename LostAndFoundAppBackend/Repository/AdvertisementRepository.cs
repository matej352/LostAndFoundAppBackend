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
            var adsWithItems = context.Advertisement.Join(context.Item,
                p => p.AdvertisementId,
                e => e.AdvertisementId,
                (p, e) => new AdvertisementWithItem { 
                                status = p.Status,
                                accountId = p.AccountId,
                                advertisementId = p.AdvertisementId,
                                creationDate = p.CreationDate,
                                item = new ItemDto { 
                                            itemId = e.ItemId,
                                            title = e.Title,
                                            description = e.Description,
                                            pictureUrl = e.PictureUrl,
                                            findingDate = (DateTime)e.FindingDate,
                                            lossDate = (DateTime)e.LossDate,
                                            AdvertisementId = e.AdvertisementId
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
                CreationDate = DateTime.UtcNow,
                AccountId = adv.accountId,
            };

            context.Add(savedAdv);
            await context.SaveChangesAsync();
            return await Task.FromResult(savedAdv.AdvertisementId);
        }

        
    }
}
