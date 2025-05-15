using Learning_Academy.Models;
using System.ComponentModel.DataAnnotations;

namespace Learning_Academy.DTO
{

    public class ChatMessageDto
    {
        public string ReceiverId { get; set; }
        public string Content { get; set; }
    }

    public class ChatMessageResponseDto
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }

    public class ConversationDto
    {
        public string OtherUserId { get; set; }
        public string OtherUserName { get; set; } // Assuming User has a Name property
        public ChatMessageResponseDto LastMessage { get; set; }
        public int UnreadCount { get; set; }
    }
}
