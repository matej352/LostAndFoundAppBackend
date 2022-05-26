using LostAndFoundAppBackend.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using EF.Model;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace LostAndFoundAppBackend.Repository
{

    public class AccountRepository : IAccountRepository
    {

        private readonly LostandfoundappdbContext context;

        public AccountRepository(LostandfoundappdbContext context)
        {
            this.context = context;
        }


        public async Task<IEnumerable<Account>> GetAll(int startIndex, int endIndex)
        {

            var accounts = context.Account.Skip(startIndex).Take(endIndex - startIndex);

            return await Task.FromResult(accounts);
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

        public async Task<int> getIdForUsername(string username)
        {
            var acc = context.Account.Where(acc => acc.Username == username).SingleOrDefault();

            return await Task.FromResult(acc.AccountId);
        }


        public async Task<int> save(RegisterDto acc)
        {
            var newAccount = new Account
            {
                Username = acc.Username,
                PhoneNumber = acc.PhoneNumber,
                Password = acc.Password,
                PasswordHashSalt = acc.PasswordHashSalt,
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

            //var passwordHash = hashPassword(dto.Password, acc.PasswordHashSalt);

            //acc.Username = dto.Username;   username se ne može promijeniti
            acc.PhoneNumber = dto.PhoneNumber;
            //acc.Password = passwordHash;
            acc.Email = dto.Email;
            acc.FirstName = dto.FirstName;
            acc.LastName = dto.LastName;
            acc.Active = dto.Active;
            await context.SaveChangesAsync();
        }

        public async Task UpdateConnId(ConnectionIdDto dto, int accId)
        {
            var acc = await context.Account.FindAsync(accId);

            acc.ConnectionId = dto.connId;
            
            await context.SaveChangesAsync();
        }

        public async Task<string> GetConnId(int accId)
        {
            var connId = context.Account.Where(a => a.AccountId == accId).Select(a => a.ConnectionId).SingleOrDefault();
            return await Task.FromResult(connId);
        }


        public async Task Delete(int id)
        {
            var acc = await context.Account.FindAsync(id);
            context.Remove(acc);

            await context.SaveChangesAsync();
        }



        private byte[] hashPassword(string password, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA256(passwordSalt))
            {
                return hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                
            }
        }




    }

}