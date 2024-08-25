using Microsoft.AspNetCore.Identity;

namespace BankingControlAPI.Domain.Entites
{
    public class Client : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PersonalID { get; set; }
        public string? AvatarPath { get; set; }
        public bool IsMale { get; set; }
        public Address? Address { get; set; }

        public required ICollection<ClientAccount> Accounts { get; set; }
    }
}
