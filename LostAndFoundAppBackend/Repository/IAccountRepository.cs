using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using EF.Model;
using LostAndFoundAppBackend.DTOs;

namespace LostAndFoundAppBackend.Repository
{
    public interface IAccountRepository
    {
        public Task<IEnumerable<Account>> GetAll();

        public Task<ActionResult<Account>> findById(int id);

        public Task<int> getIdForUsername(string username);

        public Task<int> save(RegisterDto account);

        public Task Update(UpdateAccountDto account);
        public Task UpdateConnId(ConnectionIdDto dto, int accId);
        public Task<string> GetConnId( int accId);


        public Task Delete(int id);

    }

}