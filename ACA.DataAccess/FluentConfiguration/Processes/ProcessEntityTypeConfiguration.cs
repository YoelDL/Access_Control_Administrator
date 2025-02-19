using ACA.Domain.Entities.Processes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ACA.DataAccess.FluentConfiguration.Processes
{
    public class ProcessEntityTypeConfiguration : IEntityTypeConfiguration<Process>
    {
        public void Configure(EntityTypeBuilder<Process> builder)
        {
            builder.ToTable("Processes");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.ProcessName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.ProductName)
                .IsRequired()
                .HasMaxLength(100);

            // Relación muchos a muchos con Operator a través de Process_Operator
            builder.HasMany(p => p.Processes_Operators)
                .WithOne(po => po.Process) // Aquí debes especificar cómo se relaciona Process_Operator con Process
                .HasForeignKey(po => po.ProcessId); // Clave foránea en Process_Operator

            // Relación muchos a muchos con Supervisor a través de Process_Supervisor
            builder.HasMany(p => p.Processes_Supervisors)
                .WithOne(ps => ps.Process) // Aquí debes especificar cómo se relaciona Process_Supervisor con Process
                .HasForeignKey(ps => ps.ProcessId); // Clave foránea en Process_Supervisor
        }
    }
}
