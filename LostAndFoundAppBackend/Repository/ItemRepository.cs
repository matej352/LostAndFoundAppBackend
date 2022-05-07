using EF.Model;
using LostAndFoundAppBackend.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFoundAppBackend.Repository
{
    public class ItemRepository : IItemRepository
    {
        private readonly LostandfoundappdbContext context;

        public ItemRepository(LostandfoundappdbContext context)
        {
            this.context = context;
        }


        public async Task<ActionResult<Item>> findById(int newItem_id)
        {
            var item = await context.Item.FindAsync(newItem_id);

            return await Task.FromResult(item);
        }


        public async Task<int> save(CreateItemDto item)
        {
            //INACE PROMJENIT DA FINDING I LOSS DATE SU ONI KOJE JE KORISNIK UNIO, A NE UTC NOW
            Item savedItem = new Item
            {
                Title = item.title,
                Description = item.description,
                
                FindingDate = DateTime.UtcNow,
                LossDate = DateTime.UtcNow,
                AdvertisementId = item.AdvertisementId,
                CategoryId = item.categoryId,

            };

            context.Add(savedItem);
            await context.SaveChangesAsync();
            return await Task.FromResult(savedItem.ItemId);
        }



    }
}
