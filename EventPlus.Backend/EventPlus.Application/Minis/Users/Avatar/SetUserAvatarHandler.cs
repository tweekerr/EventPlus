using EventPlus.Application.Minis.Base;
using EventPlus.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NeerCore.Exceptions;

namespace EventPlus.Application.Minis.Users.Avatar;

public class SetUserAvatarHandler(IServiceProvider serviceProvider) : MinisHandler<SetUserAvatarRequest, string>(serviceProvider)
{
    protected override async Task<string> Process(SetUserAvatarRequest request, CancellationToken ct)
    {
        var user = await Database.Set<AppUser>().SingleOrDefaultAsync(u => u.Id == UserProvider.UserId, ct);

        if (user is null)
            throw new ValidationFailedException("No such user");

        string newAvatar = string.Empty;
        
        if (request.File is not null)
        {
            newAvatar = await LoadImageToS3Bucket(request.File);
        }

        if (request.Link is not null)
        {
            newAvatar = request.Link;
        }

        user.Avatar = newAvatar;

        await Database.SaveChangesAsync(ct);

        return newAvatar;
    }

    private async Task<string> LoadImageToS3Bucket(IFormFile requestFile)
    {
        return "https://file.url";
    }
}