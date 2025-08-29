using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Carts");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

            builder.Property(c => c.UserId)
                .HasColumnType("uuid")
                .IsRequired();

            builder.Property(c => c.TotalSalePrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(c => c.BranchSaleId)
                .HasColumnType("uuid")
                .IsRequired();

            builder.Property(c => c.BranchName)
                .HasMaxLength(120)
                .IsRequired();

            builder.HasMany(c => c.CartItems)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
