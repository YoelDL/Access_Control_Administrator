using ACA.Domain.Common;
using ACA.Domain.Entities.Types;
using ACA.Domain.Relations;

namespace ACA.Domain.Entities.Workers
{
    public class Supervisor : Entity
    {
        #region Properties
        /// <summary>
        /// Nombre del Supervisor
        /// </summary>
        public string Supervisor_Name { get; set; } 
        /// <summary>
        /// Numero de identidad del Supervisor
        /// </summary>
        public string CI { get; set; }
        /// <summary>
        /// Id herdedado de la clase entity para la gestion de relaciones
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// nivel de escoalridad
        /// </summary>
        public SchoolLevel schoolLevel { get; set; }
        /// <summary>
        /// Operadores supervisados
        /// </summary>
        public ICollection<Operator> Operators { get; set; }
        /// <summary>
        /// Lista de procesos que maneja un supervisor
        /// </summary>
        public ICollection<Process_Supervisor> Processes_Supervisors { get; set; }
        #endregion


        #region Constructors
        /// <summary>
        /// Requerido por EntityFramework
        /// </summary>
        protected Supervisor() { }

        /// <summary>
        /// Inicializa un objeto de tipo Supervisor por parametros
        /// </summary>
        /// <param name="supervisor_name"></param>
        /// <param name="ci"></param>
        /// <param name="id"></param>
        /// <param name="operators"></param>
        /// <param name="processes_supervisors"></param>
        public Supervisor(
            string supervisor_name, 
            string ci, 
            SchoolLevel school_level, 
            Guid id, 
            ICollection<Operator> operators,
            ICollection<Process_Supervisor> processes_supervisors) :base(id)
        { 
            Supervisor_Name = supervisor_name;
            CI = ci;
            schoolLevel = school_level;
            Id = id;
            Operators = operators;
            Processes_Supervisors = processes_supervisors;
        }

       
        #endregion
        //public override string ToString()
        //{
        //    return $"{Supervisor_Name}, {CI}, {School_Level}, {Id}, {Operators}, {Supervisor_Processes}";
        //}

    }
}
