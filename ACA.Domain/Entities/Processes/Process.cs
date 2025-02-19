using ACA.Domain.Common;
using ACA.Domain.Relations;

namespace ACA.Domain.Entities.Processes
{
    public class Process : Entity
    {
        #region Properties
        /// <summary>
        /// Nombre del proceso 
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// Producto obtenido del proceso 
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Operadores implicados en el proceso
        /// </summary>
        public ICollection<Process_Operator> Processes_Operators { get; set; }

        /// <summary>
        /// Supervisores implicados en el proceso 
        /// </summary>
        public ICollection<Process_Supervisor> Processes_Supervisors { get; set; }
        public Guid Id { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Requerido por EntityFrameworkCore 
        /// </summary>
        protected Process() { }

        /// <summary>
        /// Inicializa un objeto de tipo Process por parámetros <see cref="Process"/>
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="productName"></param>
        /// <param name="id"></param>
        /// <param name="operators"></param>
        /// <param name="supervisors"></param>
        public Process(
            string processName,
            string productName,
            Guid id,
            ICollection<Process_Operator> processes_operators,
            ICollection<Process_Supervisor> processes_supervisors) : base(id)
        {
            ProcessName = processName;
            ProductName = productName;
            Processes_Operators = processes_operators;
            Processes_Supervisors = processes_supervisors;
        }
        #endregion

        //public override string ToString()
        //{
        //    return $"{ProcessName}, {ProductName}, {Id}, {Operators}, {Supervisors}";
        //}
    }
}


