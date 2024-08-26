namespace BankingControlAPI.Domain.Entites
{
    public class ClientAccount
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public Guid ClientId { get; set; }
        public virtual Client? Client { get; set; }
    }
}
