using Microsoft.AspNetCore.SignalR;

namespace FitJournal.Web.Hubs;

public class WorkoutTimerHub : Hub
{
    // Store active timer sessions
    private static readonly Dictionary<string, TimerState> ActiveTimers = new();

    public async Task JoinTimerSession(string sessionId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
        
        if (ActiveTimers.TryGetValue(sessionId, out var state))
        {
            await Clients.Caller.SendAsync("TimerStateUpdated", state);
        }
    }

    public async Task LeaveTimerSession(string sessionId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, sessionId);
    }

    public async Task StartTimer(string sessionId, int durationSeconds)
    {
        var state = new TimerState
        {
            SessionId = sessionId,
            TotalSeconds = durationSeconds,
            RemainingSeconds = durationSeconds,
            IsRunning = true,
            StartedAt = DateTime.UtcNow
        };
        
        ActiveTimers[sessionId] = state;
        await Clients.Group(sessionId).SendAsync("TimerStarted", state);
    }

    public async Task PauseTimer(string sessionId)
    {
        if (ActiveTimers.TryGetValue(sessionId, out var state))
        {
            state.IsRunning = false;
            state.PausedAt = DateTime.UtcNow;
            await Clients.Group(sessionId).SendAsync("TimerPaused", state);
        }
    }

    public async Task ResumeTimer(string sessionId)
    {
        if (ActiveTimers.TryGetValue(sessionId, out var state))
        {
            state.IsRunning = true;
            state.PausedAt = null;
            await Clients.Group(sessionId).SendAsync("TimerResumed", state);
        }
    }

    public async Task StopTimer(string sessionId)
    {
        ActiveTimers.Remove(sessionId);
        await Clients.Group(sessionId).SendAsync("TimerStopped");
    }

    public async Task UpdateTimerTick(string sessionId, int remainingSeconds)
    {
        if (ActiveTimers.TryGetValue(sessionId, out var state))
        {
            state.RemainingSeconds = remainingSeconds;
            await Clients.Group(sessionId).SendAsync("TimerTick", remainingSeconds);
            
            if (remainingSeconds <= 0)
            {
                await Clients.Group(sessionId).SendAsync("TimerCompleted");
            }
        }
    }
}

public class TimerState
{
    public string SessionId { get; set; } = "";
    public int TotalSeconds { get; set; }
    public int RemainingSeconds { get; set; }
    public bool IsRunning { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? PausedAt { get; set; }
}
