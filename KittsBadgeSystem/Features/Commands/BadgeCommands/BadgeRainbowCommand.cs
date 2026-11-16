using CommandSystem;
using KittsBadgeSystem.Features.Database;
using KittsBadgeSystem.Features.Models;
using LabApi.Features.Wrappers;
using System;

namespace KittsBadgeSystem.Features.Commands.BadgeCommands;

internal sealed class BadgeRainbowCommand : ICommand
{
    public string Command => "rainbow";
    public string[] Aliases => ["rn"];
    public string Description => "Toggle rainbow mode";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        BadgeData data = Player.Get(sender).GetBadgeData();

        data.RainbowEnabled = !data.RainbowEnabled;

#if !MONGODB
        DatabaseJson.SaveJsonCache();
#endif

        response = $"Rainbow {(data.RainbowEnabled ? "enabled" : "disabled")}.";

        Log.Debug("BadgeRainbowCommand", Player.Get(sender)?.DisplayName);
        return true;
    }
}
