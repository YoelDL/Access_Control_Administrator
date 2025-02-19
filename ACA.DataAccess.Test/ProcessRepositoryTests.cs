using ACA.DataAccess.Repositories;
using ACA.DataAccess.Test.Common;
using ACA.Domain.Relations;
using ACA.Domain.Entities.Processes;

namespace ACA.UnitTests
{
    [TestClass]
    public class ProcessRepositoryTests : ConnectionStringProvider
    {
        private ProcessRepository _repository;

        [TestInitialize]
        public new void Setup()
        {
            base.Setup();
            _repository = new ProcessRepository(_context);
        }

        [TestMethod]
        public void Can_Add_Process()
        {
            // Arrange
            var process = new Process("Process A", "Product X", Guid.NewGuid(), new List<Process_Operator>(), new List<Process_Supervisor>());

            // Act
            _repository.AddProcess(process);

            // Assert
            var loadedProcess = _repository.GetProcessById<Process>(process.Id);
            Assert.IsNotNull(loadedProcess, "El proceso no se cargó correctamente después de agregar.");
        }

        [TestMethod]
        public void Cannot_Get_Process_By_Invalid_Id()
        {
            // Act
            var loadedProcess = _repository.GetProcessById<Process>(Guid.NewGuid());

            // Assert
            Assert.IsNull(loadedProcess, "Se encontró un proceso con un ID inválido.");
        }

        [TestMethod]
        public void Can_Get_All_Processes()
        {
            // Arrange
            var process1 = new Process("Process A", "Product X", Guid.NewGuid(), new List<Process_Operator>(), new List<Process_Supervisor>());
            var process2 = new Process("Process B", "Product Y", Guid.NewGuid(), new List<Process_Operator>(), new List<Process_Supervisor>());

            _repository.AddProcess(process1);
            _repository.AddProcess(process2);

            // Act
            var allProcesses = _repository.GetAllProcesses<Process>().ToList();

            // Assert
            Assert.AreEqual(2, allProcesses.Count(), "La cantidad de procesos no es la esperada.");
        }

        [TestMethod]
        public void Can_Update_Process()
        {
            // Arrange
            var process = new Process("Old Process Name", "Product X", Guid.NewGuid(), new List<Process_Operator>(), new List<Process_Supervisor>());
            _repository.AddProcess(process);

            // Act
            process.ProcessName = "Updated Process Name";
            _repository.UpdateProcess(process);

            // Assert
            var updatedProcess = _repository.GetProcessById<Process>(process.Id);
            Assert.AreEqual("Updated Process Name", updatedProcess.ProcessName, "El nombre del proceso no se actualizó correctamente.");
        }

        [TestMethod]
        public void Can_Delete_Process()
        {
            // Arrange
            var process = new Process("Delete Me", "Product X", Guid.NewGuid(), new List<Process_Operator>(), new List<Process_Supervisor>());
            _repository.AddProcess(process);

            // Act
            _repository.DeleteProcess(process);

            // Assert
            var loadedProcess = _repository.GetProcessById<Process>(process.Id);
            Assert.IsNull(loadedProcess, "El proceso no se eliminó correctamente.");
        }
    }
}
