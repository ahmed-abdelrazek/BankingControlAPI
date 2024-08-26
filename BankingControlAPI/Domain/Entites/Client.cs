namespace BankingControlAPI.Domain.Entites
{
    public class Client
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? MobileNumber { get; set; }
        public required string PersonalID { get; set; }
        public string? PhotoPath { get; set; }
        public bool IsMale { get; set; }
        public Address? Address { get; set; }

        public required ICollection<ClientAccount> Accounts { get; set; }
    }
}
