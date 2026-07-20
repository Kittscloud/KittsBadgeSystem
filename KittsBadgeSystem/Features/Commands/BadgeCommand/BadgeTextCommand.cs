using CommandSystem;
using KittsBadgeSystem.Features.Database;
using LabApi.Features.Wrappers;
using System;

namespace KittsBadgeSystem.Features.Commands.BadgeCommand;

internal sealed class BadgeTextCommand : ICommand
{
    public string Command => "text";
    public string[] Aliases => ["tx", "te"];
    public string Description => "Set your badge text";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count == 0)
        {
            response = "Usage: badge text <text>";
            return false;
        }

        Player.Get(sender).GetBadgeData().BadgeText = string.Join(" ", arguments);

#if !MONGODB
        DatabaseJson.SaveJsonCache();
#endif

        response = "Badge text updated.";

        Log.Debug("BadgeTextCommand", Player.Get(sender)?.DisplayName);
        return true;
    }
}