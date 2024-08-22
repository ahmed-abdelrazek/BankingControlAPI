using Microsoft.AspNetCore.Identity;

namespace BankingControlAPI.Domain.Entites
{
    public class Client : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PersonalId { get; set; }
        public string? ProfilePhotoPath { get; set; }
        public bool Sex { get; set; }
        public Address? Address { get; set; }

        public required ICollection<ClientAccount> Accounts { get; set; }
    }
}
