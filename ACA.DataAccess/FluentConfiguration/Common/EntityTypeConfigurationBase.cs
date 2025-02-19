using ACA.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ACA.DataAccess.FluentConfiguration.Common
{
    public abstract class EntityTypeConfigurationBase<T>
         : IEntityTypeConfiguration<T>
         where T : Entity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();
        }
    }
}
