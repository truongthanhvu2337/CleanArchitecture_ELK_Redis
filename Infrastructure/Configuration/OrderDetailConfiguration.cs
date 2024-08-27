using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder
        .HasKey(od => new { od.ProductId, od.OrderId });

            builder
                .HasOne(bc => bc.Order)
                .WithMany(b => b.OrderDetails)
                .HasForeignKey(bc => bc.OrderId);

            builder
                .HasOne(bc => bc.Product)
                .WithMany(c => c.OrderDetails)
                .HasForeignKey(bc => bc.ProductId);
        }
    }
}
