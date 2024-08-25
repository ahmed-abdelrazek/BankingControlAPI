namespace BankingControlAPI.Features.Clients.DTOs
{
    public sealed class ClientDetailsDto
    {
        public required string Id { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PersonalID { get; set; }
        public required string MobileNumber { get; set; }
        public string? ProfilePhoto { get; set; }
        public required string Sex { get; set; }
        public string? Address { get; set; }
        public required IEnumerable<ClientAccountDto> Accounts { get; set; }
    }
}
