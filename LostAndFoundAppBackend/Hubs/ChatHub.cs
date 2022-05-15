using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EF.Model;
using LostAndFoundAppBackend.DTOs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace LostAndFoundAppBackend.Hubs
{
    public class ChatHub : Hub
    {

        private readonly LostandfoundappdbContext context;

        public ChatHub(LostandfoundappdbContext context)
        {
            this.context = context;
        }


        public override Task OnConnectedAsync()
        {
            Console.WriteLine("Connection established " + Context.ConnectionId);

            //slanje klijentu sa određenim connection ID-em njegov taj ID, RecieveConnId je funkcija koja će se pozvati na frontu
            Clients.Client(Context.ConnectionId).SendAsync("RecieveConnId", Context.ConnectionId);
            return base.OnConnectedAsync();

        }

        public async Task SendMessageAsync(MessageDto message) 
        {
            

           

            var toClient = context.Account.Where(a => a.AccountId == message.recieverId).Select(a => a.ConnectionId).SingleOrDefault();


            Console.WriteLine("Message recieved on: " + Context.ConnectionId);

            if (toClient == string.Empty)
            {
                Console.WriteLine("problem, no user is reciever");
            }
            else 
            {
                //save message into database
                await save(message); 


                //tocno odredenom korisniku salje se poruka i naziv metode koja će se na frontu pozvati
                await Clients.Client(toClient).SendAsync("RecieveMessage", message);
            }
        }


        public async Task save(MessageDto mess) {

            var accountId = context.Account.Where(a => a.Username == mess.From).Select(a => a.AccountId).SingleOrDefault();

            Message newMessage = new Message
            {
                
                    Content = mess.content,
                    RecieverId = mess.recieverId,
                    SendDateTime = DateTime.UtcNow,
                    IsRead = 0,
                    ReadDateTime = null,
                    AccountId = accountId

            };

            context.Add(newMessage);
            await context.SaveChangesAsync();

        }
    }
}
