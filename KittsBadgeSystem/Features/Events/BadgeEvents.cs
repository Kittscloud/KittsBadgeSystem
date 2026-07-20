#if MONGODB
using KittsBadgeSystem.Features.Database;
using KittsBadgeSystem.Features.Types;
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

    public override void OnPlayerJumped(PlayerJumpedEventArgs ev)
    {
        ev.Player.GetBadgeData().BadgeColour = ColourTypes.Random().EnumColour;
    }
}
#endif