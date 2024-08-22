using BankingControlAPI.Domain.Entites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankingControlAPI.Data
{
    public class BankingDbContext : IdentityDbContext<Client>
    {
        public BankingDbContext(DbContextOptions<BankingDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ClientAccount> ClientsAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Client>(entity =>
            {
                entity.ToTable("Clients");

                entity.Property(x => x.AvatarPath).HasMaxLength(250);

                entity.HasIndex(x => x.PhoneNumber);
                entity.Property(x => x.PhoneNumber).HasMaxLength(25);

                entity.HasIndex(x => x.FirstName);
                entity.Property(x => x.FirstName).HasMaxLength(60);

                entity.HasIndex(x => x.LastName);
                entity.Property(x => x.LastName).HasMaxLength(60);

                entity.HasIndex(x => x.NationalID);
                entity.Property(x => x.NationalID).HasMaxLength(11);

                entity.OwnsOne(o => o.Address, mo =>
                {
                    mo.ToJson();
                    mo.Property(x => x.Country).HasMaxLength(100);
                    mo.Property(x => x.City).HasMaxLength(100);
                    mo.Property(x => x.Street).HasMaxLength(100);
                    mo.Property(x => x.ZipCode).HasMaxLength(25);
                });
            });

            builder.Entity<ClientAccount>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()").ValueGeneratedOnAdd();

                entity.HasIndex(x => x.Name);
                entity.Property(x => x.Name).HasMaxLength(100);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
