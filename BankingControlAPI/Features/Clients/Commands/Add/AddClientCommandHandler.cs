using AutoMapper;
using BankingControlAPI.Data;
using BankingControlAPI.Domain.Entites;
using BankingControlAPI.Features.Clients.DTOs;
using MediatR;
using System.Net.Mime;

namespace BankingControlAPI.Features.Clients.Commands.Add
{
    internal sealed class AddClientCommandHandler(IWebHostEnvironment WebHostEnvironment, BankingDbContext DbContext, IMapper Mapper) : IRequestHandler<AddClientCommand, ClientDetailsDto>
    {
        public async Task<ClientDetailsDto> Handle(AddClientCommand request, CancellationToken cancellationToken)
        {
            var client = Mapper.Map<AddClientCommand, Client>(request);

            using var transaction = await DbContext.Database.BeginTransactionAsync(cancellationToken);

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

                        client.PhotoPath = $"images/{photoName}";
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

            try
            {
                await DbContext.AddAsync(client, cancellationToken);
                await DbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return Mapper.Map<ClientDetailsDto>(client);
            }
            catch
            {
                DeleteImageOnFail(photoFullPath);
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
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
