using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Database.Mapping;

internal sealed class CoffeeOptionMapping : IEntityTypeConfiguration<CoffeeOption>
{
    public void Configure(EntityTypeBuilder<CoffeeOption> builder)
    {
        builder.ToTable("coffee_options");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Type).HasConversion<int>().IsRequired();
        builder.Property(x => x.Size).HasConversion<int>().IsRequired();
        builder.Property(x => x.PriceInCents).IsRequired();
        builder.HasIndex(x => new { x.CoffeeId, x.Type, x.Size }).IsUnique();
    }
}
