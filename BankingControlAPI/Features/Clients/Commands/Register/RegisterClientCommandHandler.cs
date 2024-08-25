using AutoMapper;
using BankingControlAPI.CustomExceptions;
using BankingControlAPI.Domain.Entites;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net.Mime;

namespace BankingControlAPI.Features.Clients.Commands.Register
{
    internal sealed class RegisterClientCommandHandler(IWebHostEnvironment WebHostEnvironment, UserManager<Client> UserManager, IMapper Mapper) : IRequestHandler<RegisterClientCommand, string>
    {
        public async Task<string> Handle(RegisterClientCommand request, CancellationToken cancellationToken)
        {
            var user = Mapper.Map<RegisterClientCommand, Client>(request);

            string? photoFullPath = null;

            if (request.Photo is { })
            {
                if (request.Photo.Length > 1024 * 1024 * 5)
                {
                    throw new InvalidOperationException("Uploaded image exceeds 5 MB.");
                }

                if (_imagesMimes.Contains(request.Photo.ContentType))
                {
                    var imagesDirectory = Path.Combine(WebHostEnvironment.WebRootPath, "images");

                    if (Directory.Exists(imagesDirectory))
                    {
                        var extension = Path.GetExtension(request.Photo.FileName);
                        var photoName = string.Format("{0}{1}", Path.GetRandomFileName(), extension);

                        user.AvatarPath = $"images/{photoName}";
                        var photoStream = request.Photo.OpenReadStream();
                        photoFullPath = Path.Combine(imagesDirectory, photoName);

                        photoStream.Seek(0, SeekOrigin.Begin);

                        using var stream = new FileStream(photoFullPath, FileMode.Create);
                        await photoStream.CopyToAsync(stream, cancellationToken);
                    }
                }
                else
                {
                    throw new InvalidOperationException("Uploaded file is not an image.");
                }
            }

            var userRslt = await UserManager.CreateAsync(user, request.Password);

            if (!userRslt.Succeeded)
            {
                DeleteImageOnFail(photoFullPath);
                throw new IdentityStandardException("Could not create new user.", userRslt.Errors);
            }

            var roleRslt = await UserManager.AddToRoleAsync(user, request.Role.ToString());
            if (!roleRslt.Succeeded)
            {
                DeleteImageOnFail(photoFullPath);
                throw new IdentityStandardException("Could not add role to the new user.", roleRslt.Errors);
            }

            return user.Id;
        }

        private static void DeleteImageOnFail(string? fullPath)
        {
            if (!string.IsNullOrEmpty(fullPath) && File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        private static readonly HashSet<string> _imagesMimes =
        [
            MediaTypeNames.Image.Avif,
            MediaTypeNames.Image.Bmp,
            MediaTypeNames.Image.Gif,
            MediaTypeNames.Image.Icon,
            MediaTypeNames.Image.Jpeg,
            MediaTypeNames.Image.Png,
            MediaTypeNames.Image.Svg,
            MediaTypeNames.Image.Tiff,
            MediaTypeNames.Image.Webp
        ];
    }
}
