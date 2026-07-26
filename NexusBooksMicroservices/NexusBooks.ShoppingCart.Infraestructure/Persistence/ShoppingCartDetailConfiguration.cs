using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexusBooks.ShoppingCart.Domain.Models;

namespace NexusBooks.ShoppingCart.Infraestructure.Persistence;

internal sealed class ShoppingCartDetailConfiguration : IEntityTypeConfiguration<ShoppingCartDetailSessionModel>
{
    public void Configure(EntityTypeBuilder<ShoppingCartDetailSessionModel> builder)
    {
        builder.ToTable("ShoppingCartDetailSession");

        builder.HasKey(detail => detail.Id);
        builder.Property(detail => detail.Id).ValueGeneratedNever();
        builder.Property(detail => detail.CreateAt).HasDefaultValueSql("CURRENT_TIMESTAMP").IsRequired();
        builder.Property(detail => detail.ProductSelected).IsRequired();
        builder.Property(detail => detail.ShoppingCartSessionId).IsRequired();
    }
}
