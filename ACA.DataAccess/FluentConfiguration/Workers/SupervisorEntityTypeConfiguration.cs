using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACA.Domain.Entities.Workers;

namespace ACA.DataAccess.FluentConfiguration.Workers
{
    public class SupervisorEntityTypeConfiguration : IEntityTypeConfiguration<Supervisor>
    {
        public void Configure(EntityTypeBuilder<Supervisor> builder)
        {
            builder.ToTable("Supervisors");
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Supervisor_Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.CI)
                .IsRequired()
                .HasMaxLength(20);

            // Relación uno a muchos con Operator
            builder.HasMany(s => s.Operators)
                .WithOne(o => o.Asigned_Supervisor)
                .HasForeignKey(o => o.AsignedSupervisorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación muchos a muchos con Process a través de Process_Supervisor
            builder.HasMany(s => s.Processes_Supervisors)
                .WithOne(ps => ps.Supervisor) // Relación definida en Process_Supervisor
                .HasForeignKey(ps => ps.SupervisorId);
        }
    }
}
