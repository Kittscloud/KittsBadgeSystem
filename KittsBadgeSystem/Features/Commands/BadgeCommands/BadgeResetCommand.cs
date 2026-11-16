using CommandSystem;
using KittsBadgeSystem.Features.Database;
using KittsBadgeSystem.Features.Models;
using LabApi.Features.Wrappers;
using System;

namespace KittsBadgeSystem.Features.Commands.BadgeCommands;

internal sealed class BadgeResetCommand : ICommand
{
    public string Command => "Reset";
    public string[] Aliases => ["r"];
    public string Description => "Reset badge";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count != 0)
        {
            response = $"Usage: badge reset";
            return false;
        }

        BadgeData data = Player.Get(sender).GetBadgeData();

        data.BadgeEnabled = false;
        data.BadgeColour = Enums.Colour.Clear;
        data.BadgeText = "";

#if !MONGODB
        DatabaseJson.SaveJsonCache();
#endif

        response = $"Reset badge";

        Log.Debug("AdminBadgeResetCommand", Player.Get(sender)?.DisplayName);
        return true;
    }
}