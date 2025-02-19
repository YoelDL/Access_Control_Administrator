using ACA.DataAccess.FluentConfiguration.Common;
using ACA.Domain.Relations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ACA.DataAccess.FluentConfiguration.Relations
{
    internal class ProcessOperatorEntityTypeConfiguration : EntityTypeConfigurationBase<Process_Operator>
    {
        public void Configure(EntityTypeBuilder<Process_Operator> builder)
        {
            builder.ToTable("Process_Operator");

            builder.HasKey(po => new { po.ProcessId, po.OperatorId }); // Clave primaria compuesta

            // Configuración de las relaciones con Process y Operator
            builder.HasOne(po => po.Process)
                .WithMany(p => p.Processes_Operators)
                .HasForeignKey(po => po.ProcessId);

            builder.HasOne(po => po.Operador)
                .WithMany(o => o.Processes_Operators)
                .HasForeignKey(po => po.OperatorId);
        }
    }
}
