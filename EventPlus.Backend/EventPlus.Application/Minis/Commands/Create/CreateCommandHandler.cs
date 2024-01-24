using EventPlus.Application.Minis.Base;
using EventPlus.Application.Minis.Commands.Models;
using EventPlus.Domain.Entities;
using EventPlus.Domain.Entities.Identity;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.Application.Minis.Commands.Create;

public class CreateCommandHandler(IServiceProvider serviceProvider)
    : MinisHandler<CreateCommandRequest, CommandModel>(serviceProvider)
{
    public override async Task<CommandModel> Handle(CreateCommandRequest request, CancellationToken ct)
    {
        // var user = await UserProvider.GetUserAsync();
        var user = await Database.Set<AppUser>().FirstAsync(u => u.Id == 1, ct);
        
        var commandEntity = request.Adapt<Command>();

        commandEntity.CreatorId = user.Id;
        commandEntity.CommandMembers = new List<CommandMember>
        {
            new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                AppUserId = user.Id,
                Avatar = user.Avatar
            }
        };

        var commandEntry = await Database.Set<Command>().AddAsync(commandEntity, ct);

        await Database.SaveChangesAsync(ct);

        return commandEntry.Entity.Adapt<CommandModel>();
    }
}