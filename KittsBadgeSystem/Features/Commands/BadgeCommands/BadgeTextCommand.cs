using CommandSystem;
using KittsBadgeSystem.Features.Database;
using KittsBadgeSystem.Features.Models;
using LabApi.Features.Wrappers;
using System;

namespace KittsBadgeSystem.Features.Commands.BadgeCommands;

internal sealed class BadgeTextCommand : ICommand
{
    public string Command => "text";
    public string[] Aliases => ["tx", "te"];
    public string Description => "Set badge text";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count == 0)
        {
            response = "Usage: badge text <text>";
            return false;
        }

        BadgeData data = Player.Get(sender).GetBadgeData();

        data.BadgeText = string.Join(" ", arguments);

#if !MONGODB
        DatabaseJson.SaveJsonCache();
#endif

        response = $"Badge text updated to {data.BadgeText}";

        Log.Debug("BadgeTextCommand", Player.Get(sender)?.DisplayName);
        return true;
    }
}