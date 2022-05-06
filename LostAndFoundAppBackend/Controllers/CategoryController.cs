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
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {


        private readonly ICategoryRepository repository;

        public CategoryController(ICategoryRepository repository)
        {
            this.repository = repository;
        }


        /// <summary>
        /// Dohvaca sve kategorije
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<CategoryDto> GetAllCategories()
        {
            var categories = repository.getAll();

            return categories;

        }

    }
}

