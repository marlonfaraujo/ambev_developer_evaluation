using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("CartItems");

            builder.HasKey(ci => ci.Id);
            builder.Property(ci => ci.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

            builder.Property(ci => ci.ProductId)
                .HasColumnType("uuid")
                .IsRequired();

            builder.Property(ci => ci.ProductName)
                .HasMaxLength(120)
                .IsRequired();

            builder.Property(ci => ci.ProductItemQuantity)
                .IsRequired();

            builder.Property(ci => ci.UnitProductItemPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(ci => ci.DiscountAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(ci => ci.TotalSaleItemPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(ci => ci.TotalWithoutDiscount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        }
    }
}
