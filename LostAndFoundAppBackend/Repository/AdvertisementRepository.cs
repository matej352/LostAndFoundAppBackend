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
