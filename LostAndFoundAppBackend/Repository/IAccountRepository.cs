using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EF.Model;
using LostAndFoundAppBackend.DTOs;

namespace LostAndFoundAppBackend.Repository
{
    public interface IAccountRepository
    {
        public Task<IEnumerable<Account>> GetAll();

        public Task<ActionResult<Account>> findById(int id);

        public Task<int> save(RegisterDto account);

        public Task Update(UpdateAccountDto account);

        public Task Delete(int id);

    }

}