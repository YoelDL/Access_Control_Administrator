using ACA.Domain.Entities.Workers;
using ACA.Domain.Entities.Processes;
using ACA.Domain.Relations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ACA.DataAccess.FluentConfiguration.Relations;
using ACA.DataAccess.FluentConfiguration.Workers;
using ACA.DataAccess.FluentConfiguration.Processes;

namespace ACA.DataAccess.Contexts
{
    public class AplicationContext : DbContext
    {
        //Región destinada a la declaración de las tablas de las entidades base
        #region Tables 
        /// <summary>
        /// Tabla de los operadores
        /// </summary>
        public DbSet<Operator>Operators { get; set; }
        /// <summary>
        /// Tabla de los supervisores
        /// </summary>
        public DbSet<Supervisor> Supervisors { get; set; }
        /// <summary>
        /// Tabla de los procesos
        /// </summary>
        public DbSet<Process> Processes { get; set; }
        /// <summary>
        /// Tabla que relaciona los procesos y los operadores
        /// </summary>
        public DbSet<Process_Operator> Processes_Operators  { get; set; }
        /// <summary>
        /// Tabla que relaciona los procesos y los supervisores
        /// </summary>
        public DbSet<Process_Supervisor> Processes_Supervisors { get; set; }
        #endregion


        #region Constructors
        /// <summary>
        /// Requerido por EntityFrameworkCore para migraciones.
        /// </summary>
        public AplicationContext()
        {
        }

        /// <summary>
        /// Inicializa un objeto <see cref="AplicationContext"/>.
        /// </summary>
        /// <param name="connectionString">
        /// Cadena de conexión.
        /// </param>
        public AplicationContext(string connectionString)
            : base(GetOptions(connectionString))
        {
        }

        /// <summary>
        /// Inicializa un objeto <see cref="AplicationContext"/>.
        /// </summary>
        /// <param name="options">
        /// Opciones del contexto.
        /// </param>
        public AplicationContext(DbContextOptions<AplicationContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Otras configuraciones
            modelBuilder.ApplyConfiguration(new OperatorEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SupervisorEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProcessEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProcessOperatorEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProcessSupervisorEntityTypeConfiguration());

            
        }

        #endregion
        #region Helpers
        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqliteDbContextOptionsBuilderExtensions.UseSqlite(new DbContextOptionsBuilder(), connectionString).Options;
        }
        #endregion

    
    /// <summary>
    /// Habilita características en tiempo de diseño de la base de datos de la aplicación.
    /// Ej: Migraciones.
    /// </summary>
    public class ApplicationContextFactory : IDesignTimeDbContextFactory<AplicationContext>
    {
        public AplicationContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AplicationContext>();

            try
            {
                var connectionString = "Data Source = AccessControlAdministrator.DB.sqlite";
                optionsBuilder.UseSqlite(connectionString);
            }
            catch (Exception)
            {
                //handle errror here.. means DLL has no sattelite configuration file.
                throw;
            }

            return new AplicationContext(optionsBuilder.Options);
        }

    }
    
}
}

