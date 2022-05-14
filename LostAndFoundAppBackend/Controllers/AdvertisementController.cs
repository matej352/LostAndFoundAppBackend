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
    [Route("[controller]")]
    [ApiController]
    public class AdvertisementController : ControllerBase
    {

        private readonly IAdvertisementRepository repository;

        public AdvertisementController(IAdvertisementRepository repository)
        {
            this.repository = repository;
        }

        
        /// <summary>
        /// Dohvaca sve aktivne oglase sa pripadnim predmetima
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<AdvertisementWithItem>> GetAllActiveAdvertisements()
        {
            var adsWithItems = await repository.GetAllActive();

            return adsWithItems;

        }

        /// <summary>
        /// Dohvaca sve oglase odredenog korisnika sa pripadnim predmetima
        /// <param name="username">Jedinstveno korisnicko ime korisnika</param>
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("GetAdvertisementsFromUser/{username}")]
        public async Task<IEnumerable<AdvertisementWithItem>> GetAllAdvertisementsFromUser(string username)
        {
            var id = await repository.findIdByUsername(username);

            var adsWithItems = await repository.GetAll(id.Value);

            return adsWithItems;

        }


        /// <summary>
        /// Dohvaca sve aktivne oglase filtrirane prema kategoriji sa pripadnim predmetima
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAdvertisementsCategoryFilter/{id}")]
        public async Task<IEnumerable<AdvertisementWithItem>> GetAllActiveCategoryFilterAdvertisements(int id)
        {
            var adsWithItems = await repository.GetAllActive(id);

            return adsWithItems;

        }


        /// <summary>
        /// Dohvaca zeljeni oglas sa pripadnim predmetom
        /// <param name="id">Jednoznacan identifikator oglasa</param>
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("GetAdvertisementWithItem/{id}")]
        public async Task<AdvertisementWithItem> GetAdvertisementWithItem(int id)
        {
            var adWithItem = await repository.GetAdvertisementWithItem(id);

            return adWithItem;

        }


        /// <summary>
        /// Dohvaca odredeni oglas
        /// </summary>
        /// <param name="id">Jednoznacan identifikator oglasa</param>
        /// <returns></returns>

        [Authorize]
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

        /// <summary>
        /// Promjena statusa izabranog oglasa
        /// </summary>
        /// <param name="advertisementId">Jedinstveni identifikator oglasa kojeg zelimo aktivirati/deaktivirati</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{advertisementId}")]
        public async Task<ActionResult> ChangeStatus(int advertisementId)
        {
            var adv = await repository.GetAdvertisementWithItem(advertisementId);

            if (adv == null)
            {
                return Problem(statusCode: StatusCodes.Status400BadRequest, detail: $"Advertisement with id = {advertisementId} does not exist.");
            }
           
            await repository.UpdateStatus(advertisementId);
            return NoContent();
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
