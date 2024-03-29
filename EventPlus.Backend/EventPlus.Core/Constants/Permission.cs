using EventPlus.Core.Attributes;

namespace EventPlus.Core.Constants;

/// <summary>
/// Required permission value pattern:
/// <code>[area].[grp].[subgrp].[mod]</code>
/// <br/>
/// <b>[area]</b> – global application area (admin/client/etc)
/// <br/>
/// <b>[grp]</b> – group inside the area (users/roles/etc)
/// <br/>
/// <b>[subgrp]</b> – sub-group of group (user-claims/article-comments/etc)
/// <br/>
/// <b>[mod]</b> – group access modifier (read-only or full manage)
/// <br/>
/// <br/>
///
/// Allowed <b>[mod]</b> values:
/// <list type="bullet">
/// <item>
///     <term>r</term><description> – grants a read-only access</description>
/// </item>
/// <item>
///     <term>m</term><description> – grants a full manage access</description>
/// </item>
/// </list>
/// </summary>
public enum CommandPermissions
{
    [CommandPermissionInfo("*", "Admin")]
    Admin = 1,
    
    [CommandPermissionInfo("ca.cmd", "Commands")]
    CommandMember = 10,
    
    [CommandPermissionInfo("ca.cmd+", "Commands")]
    ManageCommand = 11,
    
    [CommandPermissionInfo("ca.cmd.usr+", "Commands")]
    ManageCommandMembers = 12,
    
    [CommandPermissionInfo("ca.evt+", "Events")]
    ManageEvent = 21,
    
    // [PermissionInfo("mt.usr", "Users")]
    // ReadUsers = 10,

    // [PermissionInfo("mt.usr+", "Users")]
    // ManageUsers = 11,

    [CommandPermissionInfo("ca.usr.pem", "User Permissions")]
    ReadUserPermissions = 12,

    [CommandPermissionInfo("ca.usr.pem+", "User Permissions")]
    ManageUserPermissions = 13,

    [CommandPermissionInfo("ca.rol", "Roles")]
    ReadRoles = 20,

    [CommandPermissionInfo("ca.rol+", "Roles")]
    ManageRoles = 21
}