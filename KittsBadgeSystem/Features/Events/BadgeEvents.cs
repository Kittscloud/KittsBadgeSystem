#if MONGODB
using KittsBadgeSystem.Features.Database;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;

namespace KittsBadgeSystem.Features.Events;

internal sealed class BadgeEvents : CustomEventsHandler
{
    public override void OnPlayerPreAuthenticated(PlayerPreAuthenticatedEventArgs ev)
    {
        DatabaseMongo.LoadBadgeData(ev.UserId);
    }

    public override void OnPlayerLeft(PlayerLeftEventArgs ev)
    {
        ev.Player.UploadBadgeData();
    }
}
#endif