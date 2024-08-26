namespace BankingControlAPI.Features.Clients.Commands.Add
{
    public record ClientAddressCommand
    {
        public string? Country { get; init; }
        public string? City { get; init; }
        public string? Street { get; init; }
        public string? ZipCode { get; init; }
    }
}
