using Learning_Academy.DTO;
using Learning_Academy.Models;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Learning_Academy.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {

        public async Task SendMessage(string receiverId, string content)
        {
            var senderId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(senderId))
            {
                throw new HubException("User not authenticated.");
            }

            // Send message to the receiver
            await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, content);
            // Optionally, save the message to the database via repository
        }
    }
}
