using ACA.Domain.Entities.Workers;

namespace ACA.Contracts.Workers
{
    /// <summary>
    /// Describe las funcionalidades necesarias
    /// para dar persistencia a supervisores.
    /// </summary>
    public interface ISupervisorRepository
    {
        /// <summary>
        /// Añade un supervisor al soporte de datos.
        /// </summary>
        /// <param name="supervisor">Cliente a añadir.</param>
        void AddSupervisor(Supervisor supervisor);

        /// <summary>
        /// Obtiene un supervisor del soporte de datos a partir de su identificador.
        /// </summary>
        /// <typeparam name="T">Tipo de supervisor a obtener.</typeparam>
        /// <param name="id">Identificador del supervisor.</param>
        /// <returns>supervisor obtenido del soporte de datos; de no existir, <see langword="null"/>.</returns>
        Supervisor? GetSupervisorById(Guid id);

        /// <summary>
        /// Obtiene todos los supervisores del soporte de datos.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetAllSupervisors<T>() where T : Supervisor;

        /// <summary>
        /// Actualiza el valor de un supervisor en el soporte de datos.
        /// </summary>
        /// <param name="supervisor">Instancia con la información a actualizar del supervisor.</param>
        
        void UpdateSupervisor(Supervisor supervisor);

        /// <summary>
        /// Elimina un supervisor del soporte de datos
        /// </summary>
        /// <param name="supervisor">Supervisor
        /// a eliminar.</param>
        void DeleteSupervisor(Supervisor supervisor);
        T GetSupervisorById<T>(Guid id);
    }
}
