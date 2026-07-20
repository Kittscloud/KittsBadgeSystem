using CommandSystem;
using KittsBadgeSystem.Features.Database;
using KittsBadgeSystem.Features.Models;
using LabApi.Features.Wrappers;
using System;

namespace KittsBadgeSystem.Features.Commands.BadgeCommand;

internal sealed class BadgeRainbowCommand : ICommand
{
    public string Command => "rainbow";
    public string[] Aliases => ["r"];
    public string Description => "Toggle rainbow mode";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        BadgeData badge = Player.Get(sender).GetBadgeData();

        badge.RainbowEnabled = !badge.RainbowEnabled;

#if !MONGODB
        DatabaseJson.SaveJsonCache();
#endif

        response = $"Rainbow {(badge.RainbowEnabled ? "enabled" : "disabled")}.";

        Log.Debug("BadgeRainbowCommand", Player.Get(sender)?.DisplayName);
        return true;
    }
}
