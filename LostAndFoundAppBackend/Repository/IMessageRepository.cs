using EF.Model;
using LostAndFoundAppBackend.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFoundAppBackend.Repository
{
    public interface IMessageRepository
    {
        public Task<IEnumerable<Message>> GetAll(String username);

        public Task<IEnumerable<Message>> GetMessagesForGroup(GroupParticipantsDTO group);

        public Task<IEnumerable<GroupDto>> GetGroups(String username);


    }
}
