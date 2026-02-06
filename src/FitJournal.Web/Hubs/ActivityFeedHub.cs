using Microsoft.AspNetCore.SignalR;

namespace FitJournal.Web.Hubs;

public class ActivityFeedHub : Hub
{
    public async Task JoinUserFeed(string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
    }

    public async Task LeaveUserFeed(string userId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user-{userId}");
    }

    // Called by server when new activity is posted
    public async Task BroadcastActivity(string userId, ActivityUpdate activity)
    {
        await Clients.Group($"user-{userId}").SendAsync("NewActivity", activity);
    }
}

public class ActivityUpdate
{
    public string UserId { get; set; } = "";
    public string UserName { get; set; } = "";
    public string ActivityType { get; set; } = "";
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public DateTime Timestamp { get; set; }
}
