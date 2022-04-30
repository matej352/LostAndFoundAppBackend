using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EF.Model;
using LostAndFoundAppBackend.Repository;
using LostAndFoundAppBackend.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace LostAndFoundAppBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
       
        private readonly IAccountRepository repository;

        public AccountController(IAccountRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Dohvaca sve korisnicke racune
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IEnumerable<AccountDto>> GetAllAccounts()
        {
            var accounts = await repository.GetAll();

            var dtos = accounts.Select(acc => new AccountDto
            {
                AccountId = acc.AccountId,
                Username = acc.Username,
                PhoneNumber = acc.PhoneNumber, 
                Password = acc.Password, 
                Email = acc.Email,
                FirstName = acc.FirstName,
                LastName = acc.LastName,
                Role = acc.Role,
                Active = (int)acc.Active
            });

            return dtos;

        }


        /// <summary>
        /// Dohvaca odredeni korisnicki racun 
        /// </summary>
        /// <param name="AccountId">Jednoznacan identifikator korisnickog racuna</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDto>> GetAccount(int id)
        {
            var acc = await repository.findById(id);
            if (acc.Value == null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Invalid id = {id}");
            }
            return acc.Value.AsAccountDto();

        }



        /*
        /// <summary>
        /// Stvara novi korisnicki racun. Role je 0 (user), active je 1 (true)
        /// </summary>
        /// <param name="newAccount">Podaci novog korisnickog racuna. Svi podaci su obavezni</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<AccountDto>> CreateAccount([FromBody] CreateAccountDto newAccount)
        {
            int acc_id = await repository.save(newAccount);
            Account accountInRepo = (await repository.findById(acc_id)).Value;


            return CreatedAtAction(nameof(GetAccount), new { id = acc_id }, accountInRepo.AsAccountDto());
        } */



        /// <summary>
        /// Azurira podatke odgovarajuceg korisnickog racuna
        /// </summary>
        /// <param name="id">Jedinstveni identifikator korisnickog racuna koji se zeli azurirati</param>
        /// <param name="updatedAccount">Podaci zadatka za ažuriranje</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateAccount(int id, UpdateAccountDto updatedAccount)
        {
            if (id != updatedAccount.AccountId)
            {
                return Problem(statusCode: StatusCodes.Status400BadRequest, detail: $"Different ids {id} vs {updatedAccount.AccountId}");
            }
            var accountInRepo = await repository.findById(id);
            if (accountInRepo.Value == null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Invalid id = {id}");
            }
            await repository.Update(updatedAccount);
            return NoContent();
        }



        /// <summary>
        /// Brise korisnicki racun odreden s identifikatorom
        /// </summary>
        /// <param name="id">Identifikator korisnickog racuna koji se brise</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAccount(int id)
        {
            var task = await repository.findById(id);
            if (task.Value == null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Invalid id = {id}");
            }

            await repository.Delete(id);

            return NoContent();
        }




    }
}
