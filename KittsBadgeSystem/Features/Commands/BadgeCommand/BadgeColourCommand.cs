using CommandSystem;
using KittsBadgeSystem.Features.Database;
using KittsBadgeSystem.Features.Models;
using KittsBadgeSystem.Features.Types;
using LabApi.Features.Wrappers;
using System;
using System.Linq;

namespace KittsBadgeSystem.Features.Commands.BadgeCommand;

internal sealed class BadgeColourCommand : ICommand
{
    public string Command => "colour";
    public string[] Aliases => ["color", "c"];
    public string Description => "Set your badge colour";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count != 1)
        {
            response = $"Usage: badge colour <colour>\nAvailable Colours: {string.Join(", ", ColourTypes.AllValid.Keys.Select(c => c.ToString().ToLower()))}";
            return false;
        }

        if (!ColourTypes.TryGet(arguments.At(0), out ColourInfo colour))
        {
            response = $"Unknown colour.\nAvailable colours: {string.Join(", ", ColourTypes.AllValid.Keys.Select(c => c.ToString().ToLower()))}";
            return false;
        }

        Player.Get(sender).GetBadgeData().BadgeColour = colour.EnumColour;

#if !MONGODB
        DatabaseJson.SaveJsonCache();
#endif

        response = $"Badge colour set to {colour.DisplayName}.";

        Log.Debug("BadgeColourCommand", Player.Get(sender)?.DisplayName);
        return true;
    }
}
