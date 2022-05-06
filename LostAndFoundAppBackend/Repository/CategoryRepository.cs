using EF.Model;
using LostAndFoundAppBackend.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFoundAppBackend.Repository
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly LostandfoundappdbContext context;

        public CategoryRepository(LostandfoundappdbContext context)
        {
            this.context = context;
        }


        public IEnumerable<CategoryDto> getAll()
        {
            var categories = context.Category
                .Select(c => new CategoryDto
            {
                categoryId = c.CategoryId,
                name = c.Name

            });

            return categories;
        }
    }
}
