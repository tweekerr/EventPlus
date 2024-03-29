using EventPlus.Application.Minis.Commands.Avatar;
using EventPlus.Application.Minis.Users.Avatar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.Api.Controllers;

/// <summary>
/// Users controller
/// </summary>
[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController: Controller
{
    /// <summary>
    /// Set user avatar, can be file or link
    /// </summary>
    [HttpPut("avatar")]
    public async Task<string> SetAvatar(SetUserAvatarRequest request, [FromServices] SetUserAvatarHandler handler,
        CancellationToken ct)
    {
        return await handler.Handle(request, ct);
    }
}