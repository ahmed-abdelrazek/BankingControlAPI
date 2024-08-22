namespace BankingControlAPI.Domain.Entites
{
    public class ClientAccount
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? ClientId { get; set; }
        public Client? Client { get; set; }
    }
}
