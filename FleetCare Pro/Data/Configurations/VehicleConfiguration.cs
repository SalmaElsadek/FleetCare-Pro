using System;
using FleetCare_Pro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasIndex(v => v.VIN).IsUnique();
        builder.Property(v => v.PurchasePrice).HasColumnType("decimal(18,2)");
    }
}
