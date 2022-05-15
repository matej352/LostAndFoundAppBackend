using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        /// <param name="username">Jednoznacan username korisnickog racuna</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{username}")]
        public async Task<ActionResult<AccountDto>> GetAccount(string username)
        {
            var id = await repository.getIdForUsername(username);

            var acc = await repository.findById(id);
            if (acc.Value == null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Invalid id = {id}");
            }
            return acc.Value.AsAccountDto();

        }


        /// <summary>
        /// Dohvaca odredeni korisnicki racun 
        /// </summary>
        /// <param name="accId">Jednoznacan identifikator korisnickog racuna</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("GetAccountById/{accId}")]
        public async Task<ActionResult<AccountDto>> GetAccount(int accId)
        {

            var acc = await repository.findById(accId);
            if (acc.Value == null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Invalid id = {accId}");
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
        /// <param name="username">Jedinstveni username identifikator korisnickog racuna koji se zeli azurirati</param>
        /// <param name="updatedAccount">Podaci zadatka za ažuriranje</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{username}")]
        public async Task<ActionResult> UpdateAccount(string username, UpdateAccountDto updatedAccount)
        {
            var id = await repository.getIdForUsername(username);

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
        /// Azurira SignalR Connection id zeljenog korisnika
        /// </summary>
        /// <param name="username">Jedinstveni username identifikator korisnickog racuna kojem spremamo connection id</param>
        /// <param name="dto">Objekt sa novim connection id-em</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [Route("SaveConnectionId/{username}")]
        public async Task<ActionResult> SaveConnectionId(string username, ConnectionIdDto dto)
        {
            var id = await repository.getIdForUsername(username);

            var accountInRepo = await repository.findById(id);
            if (accountInRepo.Value == null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"User with id =  {id} does not exists.");
            }
            await repository.UpdateConnId(dto, id);
            return NoContent();
        }



        /// <summary>
        /// Dohvaca SignalR Connection id zeljenog korisnika
        /// </summary>
        /// <param name="accId">Jedinstveni username identifikator korisnickog racuna kojem dohvacamo connection id</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("GetConnectionId/{accId}")]
        public async Task<ActionResult<ConnectionIdDto>> GetConnectionId(int accId)
        {
           
            var accountInRepo = await repository.findById(accId);
            if (accountInRepo.Value == null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"User with id =  {accId} does not exists.");
            }
            var connId = await repository.GetConnId(accId);
            return new ConnectionIdDto { connId = connId };
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
