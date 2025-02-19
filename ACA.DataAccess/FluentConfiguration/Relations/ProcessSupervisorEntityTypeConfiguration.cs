using ACA.Domain.Relations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ACA.DataAccess.FluentConfiguration.Relations
{
    internal class ProcessSupervisorEntityTypeConfiguration : IEntityTypeConfiguration<Process_Supervisor>
    {
        public void Configure(EntityTypeBuilder<Process_Supervisor> builder)
        {
            builder.ToTable("Process_Supervisor");

            // Clave primaria compuesta
            builder.HasKey(ps => new { ps.ProcessId, ps.SupervisorId });

            // Configuración de la relación con Process
            builder.HasOne(ps => ps.Process)
                .WithMany(p => p.Processes_Supervisors)
                .HasForeignKey(ps => ps.ProcessId) // Clave foránea explícita
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de la relación con Supervisor
            builder.HasOne(ps => ps.Supervisor)
                .WithMany(s => s.Processes_Supervisors)
                .HasForeignKey(ps => ps.SupervisorId) // Clave foránea explícita
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
