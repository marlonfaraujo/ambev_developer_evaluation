using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.ToTable("Branchs");

            builder.HasKey(b => b.Id);

            builder.Property(x => x.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

            builder.Property(b => b.Name)
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(b => b.Description)
                .HasMaxLength(120);
        }
    }
}
