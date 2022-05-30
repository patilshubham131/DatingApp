using System.Collections.Generic;
using System.Threading.Tasks;
using API.Helpers;
using API.Entities;
using API.DTOs;
namespace API.Interfaces
{
    public interface IMessageRepository
    {
         void AddMessage(Message message);
         void DeleteMessage(Message message);
         Task<Message> GetMessage(int id);
         Task<PagedList<MessageDto>> GetMessagesForUser();
         Task<IEnumerable<MessageDto>> GetMessageThread(int currentUserId, int RecipientId);
         Task<bool> SaveAllAsync();
    }
}