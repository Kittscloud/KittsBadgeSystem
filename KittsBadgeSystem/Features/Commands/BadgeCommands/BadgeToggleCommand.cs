using CommandSystem;
using KittsBadgeSystem.Features.Database;
using KittsBadgeSystem.Features.Models;
using LabApi.Features.Wrappers;
using System;

namespace KittsBadgeSystem.Features.Commands.BadgeCommands;

internal sealed class BadgeToggleCommand : ICommand
{
    public string Command => "toggle";
    public string[] Aliases => ["t", "to", "tg"];
    public string Description => "Toggle badge";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        BadgeData data = Player.Get(sender).GetBadgeData();

        data.BadgeEnabled = !data.BadgeEnabled;

#if !MONGODB
        DatabaseJson.SaveJsonCache();
#endif

        response = $"Badge {(data.BadgeEnabled ? "enabled" : "disabled")}.";

        Log.Debug("BadgeToggleCommand", Player.Get(sender)?.DisplayName);
        return true;
    }
}