using System;
using FleetCare_Pro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ServiceLineItemConfiguration : IEntityTypeConfiguration<ServiceLineItem>
{
    public void Configure(EntityTypeBuilder<ServiceLineItem> builder)
    {
        builder.Property(s => s.Cost).HasColumnType("decimal(18,2)");
    }
}

