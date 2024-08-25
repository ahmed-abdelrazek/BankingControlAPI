namespace BankingControlAPI.Features.Clients.DTOs
{
    public sealed class ClientAccountDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
    }
}
