using System;
using FleetCare_Pro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class VendorServiceConfiguration : IEntityTypeConfiguration<VendorService>
{
    public void Configure(EntityTypeBuilder<VendorService> builder)
    {
        builder.HasKey(vs => new { vs.ServiceCenterId, vs.ServiceCategoryId });

        builder.HasOne(vs => vs.ServiceCenter)
            .WithMany(sc => sc.VendorServices)
            .HasForeignKey(vs => vs.ServiceCenterId);

        builder.HasOne(vs => vs.ServiceCategory)
            .WithMany(sc => sc.VendorServices)
            .HasForeignKey(vs => vs.ServiceCategoryId);
    }
}

