using CommandSystem;
using KittsBadgeSystem.Features.Database;
using KittsBadgeSystem.Features.Models;
using LabApi.Features.Wrappers;
using System;

namespace KittsBadgeSystem.Features.Commands.BadgeCommand;

internal sealed class BadgeToggleCommand : ICommand
{
    public string Command => "toggle";
    public string[] Aliases => ["t", "to", "tg"];
    public string Description => "Toggle your badge";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        BadgeData badge = Player.Get(sender).GetBadgeData();

        badge.BadgeEnabled = !badge.BadgeEnabled;

#if !MONGODB
        DatabaseJson.SaveJsonCache();
#endif

        response = $"Badge {(badge.BadgeEnabled ? "enabled" : "disabled")}.";

        Log.Debug("BadgeToggleCommand", Player.Get(sender)?.DisplayName);
        return true;
    }
}