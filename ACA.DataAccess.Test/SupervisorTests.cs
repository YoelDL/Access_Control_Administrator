using ACA.Domain.Entities.Types;
using ACA.Domain.Entities.Workers;
using ACA.DataAccess.Test.Common;

namespace ACA.UnitTests
{
    [TestClass]
    public class SupervisorRepositoryTests : ConnectionStringProvider
    {
        private SupervisorRepository _repository;

        [TestInitialize]
        public new void Setup()
        {
            base.Setup();
            _repository = new SupervisorRepository(_context);
        }

        [TestMethod]
        public void Can_Add_Supervisor()
        {
            // Arrange
            var supervisorObj = new Supervisor("Jane Doe", "987654321", SchoolLevel.MediumTecnic, Guid.NewGuid(), null, null);

            // Act
            _repository.AddSupervisor(supervisorObj);

            // Assert
            var loadedSupervisor = _repository.GetSupervisorById(supervisorObj.Id);
            Assert.IsNotNull(loadedSupervisor, "El supervisor no se cargó correctamente después de agregar.");
        }

        [TestMethod]
        public void Cannot_Get_Supervisor_By_Invalid_Id()
        {
            // Act
            var loadedSupervisor = _repository.GetSupervisorById(Guid.NewGuid());

            // Assert
            Assert.IsNull(loadedSupervisor, "Se encontró un supervisor con un ID inválido.");
        }

        [TestMethod]
        public void Can_Get_All_Supervisors()
        {
            // Arrange
            var supervisor1 = new Supervisor("John Smith", "1122334455", SchoolLevel.MediumTecnic, Guid.NewGuid(), null, null);
            var supervisor2 = new Supervisor("Alice Johnson", "5566778899", SchoolLevel.University, Guid.NewGuid(), null, null);

            _repository.AddSupervisor(supervisor1);
            _repository.AddSupervisor(supervisor2);

            // Act
            var allSupervisors = _repository.GetAllSupervisors<Supervisor>().ToList();

            // Assert 
            Assert.AreEqual(2, allSupervisors.Count(), "La cantidad de supervisores no es la esperada.");
        }

        [TestMethod]
        public void Can_Update_Supervisor()
        {
            // Arrange 
            var supervisorObj = new Supervisor("John Smith", "1122334455", SchoolLevel.MediumTecnic, Guid.NewGuid(), null, null);
            _repository.AddSupervisor(supervisorObj);

            // Act 
            supervisorObj.Supervisor_Name = "Updated Name";
            _repository.UpdateSupervisor(supervisorObj);

            // Assert 
            var updatedSupervisor = _repository.GetSupervisorById(supervisorObj.Id);
            Assert.AreEqual("Updated Name", updatedSupervisor.Supervisor_Name, "El nombre del supervisor no se actualizó correctamente.");
        }

        [TestMethod]
        public void Can_Delete_Supervisor()
        {
            // Arrange 
            var supervisorObj = new Supervisor("John Smith", "1122334455", SchoolLevel.MediumTecnic, Guid.NewGuid(), null, null);
            _repository.AddSupervisor(supervisorObj);

            // Act 
            _repository.DeleteSupervisor(supervisorObj);

            // Assert 
            var loadedSupervisor = _repository.GetSupervisorById(supervisorObj.Id);
            Assert.IsNull(loadedSupervisor, "El supervisor no se eliminó correctamente.");
        }
    }
}
