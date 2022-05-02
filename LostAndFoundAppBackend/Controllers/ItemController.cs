using EF.Model;
using LostAndFoundAppBackend.DTOs;
using LostAndFoundAppBackend.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFoundAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {

        private readonly IItemRepository repository;

        public ItemController(IItemRepository repository)
        {
            this.repository = repository;
        }



        /// <summary>
        /// Dohvaca odredeni oglas
        /// </summary>
        /// <param name="id">Jednoznacan identifikator oglasa</param>
        /// <returns></returns>

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItem(int id)
        {
            var item = await repository.findById(id);
            if (item.Value == null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Invalid id = {id}");
            }
            return item.Value.AsItemDto();

        }




        /// <summary>
        /// Stvara novi oglas popunjen sa predmetom
        /// </summary>
        /// <param name="item">Osnovne informacije oglasa</param>
        /// <returns></returns>

        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItem(CreateItemDto item)
        {
          

            int newItem_id = await repository.save(item);

            Item itemInRepo = (await repository.findById(newItem_id)).Value;


            return CreatedAtAction(nameof(GetItem), new { id = newItem_id }, itemInRepo.AsItemDto());
        }




    }
}
