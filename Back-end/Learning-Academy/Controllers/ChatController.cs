using Learning_Academy.DTO;
using Learning_Academy.Hubs;
using Learning_Academy.Models;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace Learning_Academy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatRepository _chatRepository;
        private readonly LearningAcademyContext _context;
      
        public ChatController(IChatRepository chatRepository, LearningAcademyContext context)
        {
            _chatRepository = chatRepository;
            _context = context;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessageDto messageDto)
        {
            var senderId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(senderId))
            {
                return Unauthorized();
            }

            var message = new ChatMessage
            {
                SenderId = senderId,
                ReceiverId = messageDto.ReceiverId,
                Content = messageDto.Content,
                SentAt = DateTime.UtcNow,
                IsRead = false
            };

            await _chatRepository.AddMessageAsync(message);

            return Ok(new ChatMessageResponseDto
            {
                Id = message.Id,
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId,
                Content = message.Content,
                SentAt = message.SentAt,
                IsRead = message.IsRead
            });
        }

        [HttpGet("messages/{receiverId}")]
        public async Task<IActionResult> GetMessages(string receiverId)
        {
            var senderId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(senderId))
            {
                return Unauthorized();
            }

            var messages = await _chatRepository.GetMessagesAsync(senderId, receiverId);
            var response = messages.Select(m => new ChatMessageResponseDto
            {
                Id = m.Id,
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                Content = m.Content,
                SentAt = m.SentAt,
                IsRead = m.IsRead
            });

            return Ok(response);
        }

        [HttpPut("mark-read/{messageId}")]
        public async Task<IActionResult> MarkAsRead(int messageId)
        {
            await _chatRepository.MarkAsReadAsync(messageId);
            return Ok();
        }

        [HttpGet("conversations")]
        public async Task<IActionResult> GetConversations()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var conversations = await _chatRepository.GetConversationsAsync(userId);
            var response = new List<ConversationDto>();

            foreach (var (OtherUserId, LastMessage) in conversations)
            {
                var otherUser = await _context.Users.FindAsync(OtherUserId);
                var unreadCount = await _context.ChatMessages
                    .CountAsync(m => m.ReceiverId == userId && m.SenderId == OtherUserId && !m.IsRead);

                response.Add(new ConversationDto
                {
                    OtherUserId = OtherUserId,
                    OtherUserName = otherUser?.UserName ?? "Unknown", // Adjust based on your User model
                    LastMessage = LastMessage != null ? new ChatMessageResponseDto
                    {
                        Id = LastMessage.Id,
                        SenderId = LastMessage.SenderId,
                        ReceiverId = LastMessage.ReceiverId,
                        Content = LastMessage.Content,
                        SentAt = LastMessage.SentAt,
                        IsRead = LastMessage.IsRead
                    } : null,
                    UnreadCount = unreadCount
                });
            }

            return Ok(response.OrderByDescending(c => c.LastMessage?.SentAt));
        }

        [HttpDelete("messages/{messageId}")]
        public async Task<IActionResult> DeleteMessage(int messageId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var message = await _context.ChatMessages.FindAsync(messageId);
            if (message == null)
            {
                return NotFound("Message not found.");
            }

            if (message.SenderId != userId)
            {
                return Forbid("Only the sender can delete this message.");
            }

            await _chatRepository.DeleteMessageAsync(messageId, userId);
            return NoContent();
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var count = await _chatRepository.GetUnreadCountAsync(userId);
            return Ok(new { UnreadCount = count });
        }
    }
}
