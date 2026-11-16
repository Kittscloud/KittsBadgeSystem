using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using UnityEngine;
#if MONGODB
using KittsBadgeSystem.Features.Database;
#endif

namespace KittsBadgeSystem.Features.Events;

internal sealed class BadgeEvents : CustomEventsHandler
{
#if MONGODB
    public override void OnPlayerPreAuthenticated(PlayerPreAuthenticatedEventArgs ev)
    {
        DatabaseMongo.LoadBadgeData(ev.UserId);
    }

    public override void OnPlayerLeft(PlayerLeftEventArgs ev)
    {
        ev.Player.UploadBadgeData();
    }
#endif

    public override void OnPlayerChangedBadgeVisibility(PlayerChangedBadgeVisibilityEventArgs ev)
    {
        PlayerBadgeManager.Instance.VisibilityChanged[ev.Player] = Time.time;
    }
}
