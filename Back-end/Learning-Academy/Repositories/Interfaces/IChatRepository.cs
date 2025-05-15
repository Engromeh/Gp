using Learning_Academy.DTO;
using Learning_Academy.Models;

namespace Learning_Academy.Repositories.Interfaces
{
    public interface IChatRepository
    {
        Task AddMessageAsync(ChatMessage message);
        Task<List<ChatMessage>> GetMessagesAsync(string senderId, string receiverId);
        Task MarkAsReadAsync(int messageId);
        Task<List<(string OtherUserId, ChatMessage LastMessage)>> GetConversationsAsync(string userId);
        Task DeleteMessageAsync(int messageId, string userId);
        Task<int> GetUnreadCountAsync(string userId);
    }
}