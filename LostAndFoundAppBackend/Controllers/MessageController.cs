using LostAndFoundAppBackend.DTOs;
using LostAndFoundAppBackend.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFoundAppBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {

        private readonly IMessageRepository repository;

        public MessageController(IMessageRepository repository)
        {
            this.repository = repository;
        }


        /// <summary>
        /// Dohvaca sve poruke korisnika (koje je poslao i koje je primio)
        /// </summary>
        /// <param name="username">Jednoznacan username korisnickog racuna koji dohvaca sve poruke vezane uz njega</param>
        /// <returns></returns>
       
        [HttpGet]
        [Authorize]
        [Route("GetAll/{username}")]
        public async Task<IEnumerable<MessageFromDbDto>> GetAll(String username)
        {
            var messages = await repository.GetAll(username);

            var dtos = messages.Select(message => new MessageFromDbDto
            {
                messageId = message.MessageId,
                content = message.Content,
                recieverId = message.RecieverId,
                accountId = message.AccountId,
                sendDateTime = message.SendDateTime,

            });

            return dtos;

        }


        /// <summary>
        /// Dohvaca sve grupe razgovora u kojima je korisnik sudjelovao
        /// </summary>
        /// <param name="username">Jednoznacan username korisnickog racuna koji dohvaca sve poruke vezane uz njega</param>
        /// <returns></returns>

        [HttpGet]
        [Authorize]
        [Route("GetGroups/{username}")]
        public async Task<IEnumerable<GroupDto>> GetGroups(String username)
        {
            var messageGroups = await repository.GetGroups(username);

            return messageGroups;

        }



        /// <summary>
        /// Dohvaca sve poruke za odredeni groupChat izmedu participant1 i participant2
        /// </summary>
        /// <param name="GroupParticipantsDTO">Grupni chat izmedu svaju korisnika</param>
        /// <returns></returns>

        [HttpPost]
        [Authorize]
        [Route("GetAllForGroup")]
        public async Task<IEnumerable<MessageFromDbDto>> GetAllForGroup(GroupParticipantsDTO group)
        {
            var messages = await repository.GetMessagesForGroup(group);

            var dtos = messages.Select(message => new MessageFromDbDto
            {
                messageId = message.MessageId,
                content = message.Content,
                recieverId = message.RecieverId,
                accountId = message.AccountId,
                sendDateTime = message.SendDateTime   

            });

            return dtos;

        }



    }
}
