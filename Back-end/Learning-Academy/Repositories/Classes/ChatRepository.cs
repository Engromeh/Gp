using Learning_Academy.DTO;
using Learning_Academy.Models;
using Microsoft.EntityFrameworkCore;
using Learning_Academy.Repositories.Interfaces;

namespace Learning_Academy.Repositories.Classes
{
    public class ChatRepository : IChatRepository
    {
        private readonly LearningAcademyContext _context;
        public ChatRepository(LearningAcademyContext context)
        {
            _context = context;
        }

        public async Task AddMessageAsync(ChatMessage message)
        {
            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMessageAsync(int messageId, string userId)
        {
            var message = await _context.ChatMessages.FindAsync(messageId);
            if (message != null && message.SenderId == userId)
            {
                _context.ChatMessages.Remove(message);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<(string OtherUserId, ChatMessage LastMessage)>> GetConversationsAsync(string userId)
        {
            var conversations = await _context.ChatMessages
               .Where(m => m.SenderId == userId || m.ReceiverId == userId)
               .GroupBy(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
               .Select(g => new
               {
                   OtherUserId = g.Key,
                   LastMessage = g.OrderByDescending(m => m.SentAt).FirstOrDefault()
               })
               .ToListAsync();

            return conversations.Select(c => (c.OtherUserId, c.LastMessage)).ToList();
        }

        public async Task<List<ChatMessage>> GetMessagesAsync(string senderId, string receiverId)
        {
            return await _context.ChatMessages
                .Where(m => (m.SenderId == senderId && m.ReceiverId == receiverId) ||
                            (m.SenderId == receiverId && m.ReceiverId == senderId))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public  async Task<int> GetUnreadCountAsync(string userId)
        {
            return await _context.ChatMessages
                .Where(m => m.ReceiverId == userId && !m.IsRead)
                .CountAsync();
        }

        public async Task MarkAsReadAsync(int messageId)
        {
            var message = await _context.ChatMessages.FindAsync(messageId);
            if (message != null)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
