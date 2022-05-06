using LostAndFoundAppBackend.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFoundAppBackend.Repository
{
    public interface ICategoryRepository
    {

        public IEnumerable<CategoryDto> getAll();
    }
}
