using LostAndFoundAppBackend.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using EF.Model;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFoundAppBackend.Repository
{

    public class AccountRepository : IAccountRepository
    {

        private readonly LostandfoundappdbContext context;

        public AccountRepository(LostandfoundappdbContext context)
        {
            this.context = context;
        }


        public async Task<IEnumerable<Account>> GetAll()
        {

            var accounts = context.Account;

            return await Task.FromResult(accounts);
        }


        public async Task<ActionResult<Account>> findById(int id)
        {
            var acc = await context.Account.FindAsync(id);

            return await Task.FromResult(acc);
        }


        public async Task<int> save(CreateAccountDto acc)
        {
            var newAccount = new Account
            {
                Username = acc.Username,
                PhoneNumber = acc.PhoneNumber,
                Password = acc.Password,
                Email = acc.Email,
                FirstName = acc.FirstName,
                LastName = acc.LastName,
                Role = 0,
                Active = 1
            };
            context.Add(newAccount);
            await context.SaveChangesAsync();
            return await Task.FromResult(newAccount.AccountId);
        }

        public async Task Update(UpdateAccountDto dto)
        {
            var acc = await context.Account.FindAsync(dto.AccountId);
            acc.Username = dto.Username;
            acc.PhoneNumber = dto.PhoneNumber;
            acc.Password = dto.Password;
            acc.Email = dto.Email;
            acc.FirstName = dto.FirstName;
            acc.LastName = dto.LastName;
            acc.Active = dto.Active;
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var acc = await context.Account.FindAsync(id);
            context.Remove(acc);

            await context.SaveChangesAsync();
        }




    }

}