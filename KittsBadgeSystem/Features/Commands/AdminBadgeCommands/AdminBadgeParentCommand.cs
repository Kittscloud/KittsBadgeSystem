using CommandSystem;
using KittsBadgeSystem.Features.Database;
using KittsBadgeSystem.Features.Models;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using System;

namespace KittsBadgeSystem.Features.Commands.AdminBadgeCommands;

[CommandHandler(typeof(ClientCommandHandler))]
[CommandHandler(typeof(RemoteAdminCommandHandler))]
[CommandHandler(typeof(GameConsoleCommandHandler))]
internal sealed class AdminBadgeParentCommand : ParentCommand
{
    public override string Command => "adminbadge";

    public override string[] Aliases => ["abadge", "ab"];

    public override string Description => "Manage another player's badge";

    public AdminBadgeParentCommand()
    {
        LoadGeneratedCommands();
    }

    public override void LoadGeneratedCommands()
    {
        RegisterCommand(new AdminBadgeToggleCommand());
        RegisterCommand(new AdminBadgeRainbowCommand());
        RegisterCommand(new AdminBadgeColourCommand());
        RegisterCommand(new AdminBadgeTextCommand());
        RegisterCommand(new AdminBadgeResetCommand());
    }

    protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.HasPermissions(KittsBadgeSystem.Config.AdminBadgePermission))
        {
            response = "<color=red>You do not have permission to use this command.</color>";
            return false;
        }

        if (arguments.Count != 1)
        {
            response = "Usage: abadge <player>";
            return false;
        }

        if (!int.TryParse(arguments.At(0), out int playerId) || !Player.TryGet(playerId, out Player player))
        {
            response = "Player not found";
            return false;
        }

        BadgeData data = player.GetBadgeData();

        response =
            $"Badge settings for {player.DisplayName}\n\n" +
            $"Enabled: {data.BadgeEnabled}\n" +
            $"Rainbow: {data.RainbowEnabled}\n" +
            $"Colour: {data.BadgeColour}\n" +
            $"Text: {data.BadgeText}\n\n" +
            $"Commands:\n" +
            $"abadge toggle {player.PlayerId}\n" +
            $"abadge rainbow {player.PlayerId}\n" +
            $"abadge colour {player.PlayerId} <colour>\n" +
            $"abadge text {player.PlayerId} <text>\n" +
            $"abadge reset {player.PlayerId}";

        return true;
    }
}
