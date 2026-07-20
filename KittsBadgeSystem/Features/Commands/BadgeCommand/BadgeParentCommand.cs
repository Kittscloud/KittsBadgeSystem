using CommandSystem;
using KittsBadgeSystem.Features.Database;
using KittsBadgeSystem.Features.Models;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using System;

namespace KittsBadgeSystem.Features.Commands.BadgeCommand;

[CommandHandler(typeof(ClientCommandHandler))]
[CommandHandler(typeof(RemoteAdminCommandHandler))]
internal sealed class BadgeParentCommand : ParentCommand
{
    public override string Command => "badge";

    public override string[] Aliases => ["b"];

    public override string Description => "Manage your badge";

    public BadgeParentCommand()
    {
        LoadGeneratedCommands();
    }

    public override void LoadGeneratedCommands()
    {
        RegisterCommand(new BadgeToggleCommand());
        RegisterCommand(new BadgeRainbowCommand());
        RegisterCommand(new BadgeColourCommand());
        RegisterCommand(new BadgeTextCommand());
    }

    protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.HasPermissions(KittsBadgeSystem.Config.PlayerBadgePermission))
        {
            response = "<color=red>You do not have permission to use this command.</color>";
            return false;
        }

        Player player = Player.Get(sender);
        BadgeData badge = player.GetBadgeData();

        response =
            "<b><color=yellow>Badge Settings</color></b>\n\n" +
            $"Enabled: {(badge.BadgeEnabled ? "<color=green>Yes</color>" : "<color=red>No</color>")}\n" +
            $"Rainbow: {(badge.RainbowEnabled ? "<color=green>Yes</color>" : "<color=red>No</color>")}\n" +
            $"Colour: {badge.BadgeColour}\n" +
            $"Text: {badge.BadgeText}\n\n" +
            "<b>Commands</b>\n" +
            "badge toggle\n" +
            "badge rainbow\n" +
            "badge colour <colour>\n" +
            "badge text <text>";

        Log.Debug("BadgeParentCommand", Player.Get(sender)?.DisplayName);
        return true;
    }
}