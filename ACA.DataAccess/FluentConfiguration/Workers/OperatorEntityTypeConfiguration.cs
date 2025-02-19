using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ACA.Domain.Entities.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACA.Domain.Entities.Workers;

namespace ACA.DataAccess.FluentConfiguration.Workers
{
    public class OperatorEntityTypeConfiguration : IEntityTypeConfiguration<Operator>
    {
        public void Configure(EntityTypeBuilder<Operator> builder)
        {
            builder.ToTable("Operators");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Operator_Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.CI)
                .IsRequired()
                .HasMaxLength(20);

            // Relación uno a muchos con Supervisor
            builder.HasOne(o => o.Asigned_Supervisor)
                .WithMany(s => s.Operators)
                .HasForeignKey(o => o.AsignedSupervisorId) // Usar la propiedad como clave foránea
                .OnDelete(DeleteBehavior.Restrict); // Comportamiento al eliminar

            // Configurar relación con Process_Operator (suponiendo que existe una entidad Process_Operator)
            builder.HasMany(o => o.Processes_Operators)
                .WithOne(po => po.Operador) // Aquí debes especificar cómo se relaciona Process_Operator con Operator
                .HasForeignKey(po => po.OperatorId); // Clave foránea en Process_Operator

            // Auto incluir Process_Operators si es necesario
            builder.Navigation(o => o.Processes_Operators).AutoInclude();
        }
    }
}

