using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sales");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
            /*
            builder.Property(x => x.SaleNumber)
              .UseIdentityColumn();
            */
            builder.Property(s => s.SaleNumber)
                .ValueGeneratedOnAdd();

            builder.Property(s => s.SaleDate)
                .HasColumnType("timestamp")
                .IsRequired();

            builder.Property(s => s.TotalSalePrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(s => s.SaleStatus)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Branch>()
                .WithMany()
                .HasForeignKey(s => s.BranchSaleId)
                .OnDelete(DeleteBehavior.Restrict);

            
            builder.HasMany(s => s.SaleItems)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            

        }
    }
}
