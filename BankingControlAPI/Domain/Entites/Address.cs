﻿namespace BankingControlAPI.Domain.Entites
{
    public class Address
    {
        public required string Country { get; set; }
        public required string City { get; set; }
        public required string Street { get; set; }
        public required string ZipCode { get; set; }
    }
}
