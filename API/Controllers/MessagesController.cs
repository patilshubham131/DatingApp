using Microsoft.AspNetCore.Mvc;
using API.Data;
using System.Collections.Generic;
using API.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;
using API.DTOs;
using AutoMapper;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using API.Extensions;
using API.Helpers;
namespace API.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        private readonly IUserRepository userRepository;
        private readonly IMessageRepository messageRepository;
        private readonly IMapper mapper;
        public MessagesController(IUserRepository userRepository, IMessageRepository messageRepository, IMapper mapper)
        {
            this.mapper = mapper;
            this.messageRepository = messageRepository;
            this.userRepository = userRepository;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {

            var username = User.GetUsername();

            if (username == createMessageDto.RecipientUsername.ToLower())
            {
                return BadRequest("You can't send message to yourself");
            }

            var sender = await userRepository.GetUserByUserName(username);
            var recipient = await userRepository.GetUserByUserName(createMessageDto.RecipientUsername);

            if (recipient == null) return NotFound();

            var message = new Message
            {

                Sender = sender,
                Recipient = recipient,
                SenderUserName = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            };

            messageRepository.AddMessage(message);

            if(await messageRepository.SaveAllAsync()) return Ok(mapper.Map<MessageDto>(message));

            return BadRequest("failed to send message");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery]MessageParams messageParams){

            messageParams.Username = User.GetUsername();

            var message = await messageRepository.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(message.CurrenctPage, message.PageSize, message.TotalCount, message.TotalPages);

            return message;

        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username){
            
            var currentUsername = User.GetUsername();

            return Ok(await messageRepository.GetMessageThread(currentUsername, username));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id){

            var username = User.GetUsername();

            var message = await messageRepository.GetMessage(id);

            if(message.SenderUserName != username && message.RecipientUsername != username)
                return Unauthorized();

            if(message.SenderUserName == username)
                message.SenderDeleted = true;

            if(message.RecipientUsername == username)
                message.RecipientDeleted = true;

            if(message.SenderDeleted && message.RecipientDeleted)
                messageRepository.DeleteMessage(message);

            if(await messageRepository.SaveAllAsync())
                return Ok();

            return BadRequest("problem occured while deleting the message");
        }
    }
}