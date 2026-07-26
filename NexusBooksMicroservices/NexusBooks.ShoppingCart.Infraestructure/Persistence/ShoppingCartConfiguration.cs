using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexusBooks.ShoppingCart.Domain.Models;

namespace NexusBooks.ShoppingCart.Infraestructure.Persistence;

internal sealed class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCartSessionModel>
{
    public void Configure(EntityTypeBuilder<ShoppingCartSessionModel> builder)
    {
        builder.ToTable("ShoppingCartSession");

        builder.HasKey(shoppingCart => shoppingCart.Id);
        builder.Property(shoppingCart => shoppingCart.Id).ValueGeneratedNever();
        builder.Property(shoppingCart => shoppingCart.CreateAt).HasDefaultValueSql("CURRENT_TIMESTAMP").IsRequired();

        builder.HasMany(shoppingCart => shoppingCart.Details)
            .WithOne(detail => detail.ShoppingCart)
            .HasForeignKey(detail => detail.ShoppingCartSessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
