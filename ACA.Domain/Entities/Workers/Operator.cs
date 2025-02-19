using ACA.Domain.Common;
using ACA.Domain.Entities.Types;
using ACA.Domain.Relations;

namespace ACA.Domain.Entities.Workers
{
    public class Operator : Entity
    {
        #region Properties
        /// <summary>
        /// Nombre del operador
        /// </summary>
        public string Operator_Name { get; set; } 
        /// <summary>
        /// Numero de identidad del operador 
        /// </summary>
        public string CI { get; set; }  
        /// <summary>
        /// Id herdedado de la clase entity para la gestion de relaciones
        /// </summary>
        public Guid Id { get; set; }
        public SchoolLevel schoolLevel { get; set; }
        /// <summary>
        /// Supervisor asignado al operador 
        /// </summary>
        public Guid? AsignedSupervisorId { get; set; } // Nueva propiedad para la clave externa

        /// <summary>
        /// Supervisor asignado al operador 
        /// </summary>
        
        public Supervisor Asigned_Supervisor { get; set; }
        /// <summary>
        /// Lista de procesos que maneja el operador
        /// </summary>
        public ICollection<Process_Operator> Processes_Operators { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Requerido por EntityFramework
        /// </summary>
        protected Operator () { }

        /// <summary>
        /// Inicializa un objeto de tipo Operador por parametros
        /// </summary>
        /// <param name="operatorName"></param>
        /// <param name="ci"></param>
        /// <param name="id"></param>
        /// <param name="asgined_supervisor"></param>
        /// <param name="processes_operators"></param>
        public Operator(
            string operatorName, 
            string ci, 
            Guid id, 
            SchoolLevel School_Level,
            Supervisor asgined_supervisor,
            ICollection<Process_Operator> processes_operators) :base(id) 
        { 
            Operator_Name = operatorName;
            CI = ci;
            Id = id;
            schoolLevel = School_Level;
            Asigned_Supervisor = asgined_supervisor;
            Processes_Operators = processes_operators;
        }

       
        #endregion
        //public override string ToString()
        //{
        //    return $"{Operator_Name}, {CI}, {School_Level}, {Id}, {Asigned_Supervisor}, {Operator_Processes}";
        //}
    }
}
