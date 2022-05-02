using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LostAndFoundAppBackend.Repository;
using LostAndFoundAppBackend.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using EF.Model;

namespace LostAndFoundAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementController : ControllerBase
    {

        private readonly IAdvertisementRepository repository;

        public AdvertisementController(IAdvertisementRepository repository)
        {
            this.repository = repository;
        }

        /*
        /// <summary>
        /// Dohvaca sve korisnicke racune
        /// </summary>
        /// <returns></returns>
        [Authorize]
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
        */

        /// <summary>
        /// Dohvaca odredeni oglas
        /// </summary>
        /// <param name="id">Jednoznacan identifikator oglasa</param>
        /// <returns></returns>
        
        [HttpGet("{id}")]
        public async Task<ActionResult<AdvertisementDto>> GetAdvertisement(int id)
        {
            var adv = await repository.findAdvById(id);
            if (adv.Value == null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Invalid id = {id}");
            }
            return adv.Value.AsAdvertisementDto();

        }




        /// <summary>
        /// Stvara novi oglas. Status je 1 (aktivan)
        /// </summary>
        /// <param name="newAdv">Osnovne informacije oglasa</param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public async Task<ActionResult<AdvertisementDto>> CreateAdvertisement(int id, CreateAdvertisementDto newAdv)
        {
            if (id != newAdv.accountId)
            {
                return Problem(statusCode: StatusCodes.Status400BadRequest, detail: $"Different ids {id} vs {newAdv.accountId}");
            }
            var accountInRepo = await repository.findAccById(id);
            if (accountInRepo.Value == null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Invalid id = {id}");
            }

            int newAdv_id = await repository.save(newAdv);

            Advertisement advInRepo = (await repository.findAdvById(newAdv_id)).Value;


            return CreatedAtAction(nameof(GetAdvertisement), new { id = newAdv_id }, advInRepo.AsAdvertisementDto());
        } 


        /*

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
            var accountInRepo = await repository.findAccById(id);
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
            var task = await repository.findAccById(id);
            if (task.Value == null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Invalid id = {id}");
            }

            await repository.Delete(id);

            return NoContent();
        }




        */

    }
}
