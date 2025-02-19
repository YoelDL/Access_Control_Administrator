using global::ACA.Domain.Entities.Types;
using global::ACA.Domain.Entities.Workers;
using global::ACA.Domain.Relations;
using global::ACA.DataAccess.Test.Common;

namespace ACA.UnitTests
{
    [TestClass]
        public class OperatorRepositoryTests : ConnectionStringProvider
        {
            private OperatorRepository _repository;

            [TestInitialize]
            public new void Setup()
            {
                base.Setup();
                _repository = new OperatorRepository(_context);
            }

            [TestMethod]
            public void Can_Add_Operator()
            {
                // Arrange
                var operatorObj = new Operator("John Doe", "123456789", Guid.NewGuid(), SchoolLevel.High, null, new List<Process_Operator>());

                // Act
                _repository.AddOperator(operatorObj);

                // Assert
                var loadedOperator = _repository.GetOperatorById(operatorObj.Id);
                Assert.IsNotNull(loadedOperator, "El operador no se cargó correctamente después de agregar.");
            }

            [TestMethod]
            public void Cannot_Get_Operator_By_Invalid_Id()
            {
                // Act
                var loadedOperator = _repository.GetOperatorById(Guid.NewGuid());

                // Assert
                Assert.IsNull(loadedOperator, "Se encontró un operador con un ID inválido.");
            }

            [TestMethod]
            public void Can_Get_All_Operators()
            {
                // Arrange
                var operator1 = new Operator("John Doe", "123456789", Guid.NewGuid(), SchoolLevel.High, null, new List<Process_Operator>());
                var operator2 = new Operator("Jane Smith", "987654321", Guid.NewGuid(), SchoolLevel.University, null, new List<Process_Operator>());

                _repository.AddOperator(operator1);
                _repository.AddOperator(operator2);

                // Act
                var allOperators = _repository.GetAllOperators<Operator>().ToList();

                // Assert
                Assert.AreEqual(2, allOperators.Count(), "La cantidad de operadores no es la esperada.");
            }

            [TestMethod]
            public void Can_Update_Operator()
            {
                // Arrange
                var operatorObj = new Operator("John Doe", "123456789", Guid.NewGuid(), SchoolLevel.High, null, new List<Process_Operator>());
                _repository.AddOperator(operatorObj);

                // Act
                operatorObj.Operator_Name = "Updated Name";
                _repository.UpdateOperator(operatorObj);

                // Assert
                var updatedOperator = _repository.GetOperatorById(operatorObj.Id);
                Assert.AreEqual("Updated Name", updatedOperator.Operator_Name, "El nombre del operador no se actualizó correctamente.");
            }

            [TestMethod]
            public void Can_Delete_Operator()
            {
                // Arrange
                var operatorObj = new Operator("John Doe", "123456789", Guid.NewGuid(), SchoolLevel.High, null, new List<Process_Operator>());
                _repository.AddOperator(operatorObj);

                // Act
                _repository.DeleteOperator(operatorObj);

                // Assert
                var loadedOperator = _repository.GetOperatorById(operatorObj.Id);
                Assert.IsNull(loadedOperator, "El operador no se eliminó correctamente.");
            }
        }
    }
