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
        [Route("count")]
        public async Task<int> GetAllActiveAdvertisementsCount()
        {
            List<AdvertisementWithItem> adsWithItems = await repository.GetAllActive();

            return adsWithItems.Count;

        }

        /// <summary>
        /// Dohvaca sve oglase (AKTIVNE I EXPIRED) sa pripadnim predmetima
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("allcount")]
        public async Task<int> GetAllAdvertisementsCount()
        {
            var count = await repository.GetAllCount();

            return count;

        }


        /// <summary>
        /// Dohvaca sve aktivne oglase sa pripadnim predmetima
        /// <param name="id">Id kategorije</param>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("count/{id}")]
        public async Task<int> GetAllActiveAdvertisementsCategoryFilterCount(int id)
        {
            List<AdvertisementWithItem> adsWithItems = await repository.GetAllActive(id);

            return adsWithItems.Count;

        }


        /// <summary>
        /// Dohvaca sve aktivne oglase sa pripadnim predmetima od indexa do indexa
        /// <param name="query">Objekt sa pocetnim i zavrsnim indeksom oglasa kojeg dohvacamo</param>
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("getAll")]
        public async Task<IEnumerable<AdvertisementWithItem>> GetAllActiveAdvertisements(QueryOptionsDto query)
        {
            var adsWithItems = await repository.GetAllActive(query);

            return adsWithItems;

        }

        /// <summary>
        /// Dohvaca sve oglase (AKTIVNE I EXPIRED) sa pripadnim predmetima
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("getAllAndExpired")]
        public async Task<IEnumerable<AdvertisementWithItem>> GetAllActiveAdvertisementsAndExpired(QueryOptionsDto query)
        {
            var  adsWithItems = await repository.GetAll(query);

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
        /// Dohvaca NEAKTIVNE oglase odredenog korisnika sa pripadnim predmetima
        /// <param name="username">Jedinstveno korisnicko ime korisnika</param>
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("GetInactiveAdvertisementsFromUser/{username}")]
        public async Task<IEnumerable<AdvertisementWithItem>> GetInactiveAdvertisementsFromUser(string username)
        {
            var id = await repository.findIdByUsername(username);

            var adsWithItems = await repository.GetAllInactive(id.Value);

            return adsWithItems;

        }

        /// <summary>
        /// Produzuje aktivnost oglasa za 30 dana
        /// <param name="advertidementId">Jedinstveni identifikator oglasa</param>
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("reactivate/{advertidementId}")]
        public async Task<AdvertisementDto> UpdateAdvertisementExpirationDate(int advertidementId)
        {
          
            var add = await repository.UpdateExpirationDate(advertidementId);

            return add.AsAdvertisementDto();

        }


        /// <summary>
        /// Dohvaca sve aktivne oglase filtrirane prema kategoriji sa pripadnim predmetima od indexa do indexa
        /// <param name="query">Objekt sa pocetnim i zavrsnim indeksom oglasa kojeg dohvacamo</param>
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAdvertisementsCategoryFilter/{id}")]
        public async Task<IEnumerable<AdvertisementWithItem>> GetAllActiveCategoryFilterAdvertisements(int id, QueryOptionsDto query)
        {
            var adsWithItems = await repository.GetAllActive(id, query);

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
        [HttpGet]
        public async Task<ActionResult<AdvertisementDto>> GetAdvertisement([FromQuery(Name = "param")] int id )
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
        /// <param name="username">Jedinstveno korisnicko ime korisnika koji stvara oglas</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("{username}")]
        public async Task<ActionResult<AdvertisementDto>> CreateAdvertisement(string username, CreateAdvertisementDto newAdv)
        {
           
            if (username != newAdv.username)
            {
                return Problem(statusCode: StatusCodes.Status400BadRequest, detail: $"Different usernames: {username} vs {newAdv.username}");
            }
            var id = await repository.findIdByUsername(username);
            var accountInRepo = await repository.findAccById(id.Value);
            if (accountInRepo.Value == null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Invalid username = {username}");
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


        /// <summary>
        /// Brise oglas (i pripadne mu objekte: item, image) odreden s identifikatorom
        /// </summary>
        /// <param name="advId">Identifikator oglasa koji se brise</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            var task = await repository.findAdvById(id);
            if (task.Equals(null))
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Invalid id = {id}");
            }

            await repository.Delete(id);

            return NoContent();
        }



    }
}
