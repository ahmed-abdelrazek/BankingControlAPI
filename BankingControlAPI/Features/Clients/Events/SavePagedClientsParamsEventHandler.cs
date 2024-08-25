using BankingControlAPI.Data;
using BankingControlAPI.Domain.Enums;
using MediatR;

namespace BankingControlAPI.Features.Clients.Events
{
    internal class SavePagedClientsParamsEventHandler(ILogger<SavePagedClientsParamsEvent> Logger, BankingDbContext dbContext) : INotificationHandler<SavePagedClientsParamsEvent>
    {
        public async Task Handle(SavePagedClientsParamsEvent notification, CancellationToken cancellationToken)
        {
            Logger.LogInformation("Saving latest admin attemp to filter paged clients");

            await Task.Delay(500, cancellationToken);

            try
            {
                await dbContext.FiltersParameters.AddAsync(new Domain.Entites.FilterParameter
                {
                    From = FiltersParamsId.ClientPagedFilter,
                    Content = System.Text.Json.JsonSerializer.Serialize(notification)
                }, cancellationToken);

                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error saving latest client filter params");
            }
        }
    }
}
