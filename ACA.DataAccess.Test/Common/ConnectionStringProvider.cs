using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ACA.DataAccess.Contexts;

namespace ACA.DataAccess.Test.Common
{

    public abstract class ConnectionStringProvider
    {
        protected SqliteConnection _connection;
        protected AplicationContext _context;

        [TestInitialize]
        public void Setup()
        {
            // Crear una conexión SQLite en memoria
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<AplicationContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new AplicationContext(options);
            _context.Database.EnsureCreated();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _connection.Close();
        }
    }
}

