using EF.Model;
using LostAndFoundAppBackend.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFoundAppBackend.Repository
{
    public class MessageRepository : IMessageRepository
    {

        private readonly LostandfoundappdbContext context;

        public MessageRepository(LostandfoundappdbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Message>> GetAll(string username)
        {

            var account = context.Account.Where(a => a.Username == username).SingleOrDefault();

            var messages = context.Message.Where(m => m.AccountId == account.AccountId || m.RecieverId == account.AccountId);

            return await Task.FromResult(messages);
        }

        public async Task<IEnumerable<GroupDto>> GetGroups(string username)
        {
            var accounts = context.Account.ToList();

            var account = accounts.Where(a => a.Username == username).SingleOrDefault();
            
            var messages = context.Message.Where(m => m.AccountId == account.AccountId || m.RecieverId == account.AccountId).ToList();

            List<int> groupsWithAccIdsSender = messages.Select(m => m.AccountId).ToList();
            List<int> groupsWithAccIdsReciever = messages.Select(m => m.RecieverId).ToList();

            //spajanje lista
            foreach (int id in groupsWithAccIdsReciever)
            {
                groupsWithAccIdsSender.Add(id);
            }


            var groupsWithAccIdsFiltered = groupsWithAccIdsSender.Where(id => id != account.AccountId);

            List<GroupDto> dtos = new List<GroupDto>();

            
            //nije mi poredak od novijeg prema starijem groupchatu (unatoc .reverse)
            foreach (int accId in groupsWithAccIdsFiltered.Distinct().Reverse())
            {
                var accountName = accounts.Where(a => a.AccountId == accId).Select(a => a.Username).SingleOrDefault();

                var lastMessage = messages.Where(m => m.AccountId == accId || m.RecieverId == accId)
                                            .OrderBy(m => m.SendDateTime).LastOrDefault();

                var lastMessageDto = new MessageFromDbDto
                {
                    messageId = lastMessage.MessageId,
                    content = lastMessage.Content,
                    recieverId = lastMessage.RecieverId,
                    accountId = lastMessage.AccountId,
                    sendDateTime = lastMessage.SendDateTime,
                    ReadDateTime = lastMessage.ReadDateTime,
                    isRead = lastMessage.IsRead

                };

                dtos.Add(new GroupDto { 
                    chatWith = accountName,
                    lastMessage = lastMessageDto
                });
            }


            return await Task.FromResult(dtos);

        }

        public async Task<IEnumerable<Message>> GetMessagesForGroup(GroupParticipantsDTO group)
        {

            var accounts = context.Account.ToList();

            var participant1ID = accounts.Where(a => a.Username == group.participant1).Select(a => a.AccountId).SingleOrDefault();

            var participant2ID = accounts.Where(a => a.Username == group.participant2).Select(a => a.AccountId).SingleOrDefault();


            var messages = context.Message
                .Where(m => (m.AccountId == participant1ID && m.RecieverId == participant2ID) 
                               || (m.AccountId == participant2ID && m.RecieverId == participant1ID) );

            return await Task.FromResult(messages);
        }
    }
}
