using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.ToTable("SaleItems");

            builder.HasKey(si => si.Id);
            builder.Property(x => x.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

            builder.Property(si => si.ProductItemQuantity)
                .IsRequired();

            builder.Property(si => si.UnitProductItemPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(si => si.DiscountAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(si => si.TotalSaleItemPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(si => si.TotalWithoutDiscount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(si => si.SaleItemStatus)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasOne<Product>()
                .WithMany()
                .HasForeignKey(si => si.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
