using LostAndFoundAppBackend.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EF.Model;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFoundAppBackend.Repository
{
    public interface IItemRepository
    {
        public Task<int> save(CreateItemDto item);
        public Task<ActionResult<Item>> findById(int newItem_id);
    }
}
