using ACA.Domain.Entities.Processes;

namespace ACA.Application.Contracts.Processes
{
    /// <summary>
    /// Describe las funcionalidades necesarias
    /// para dar persistencia a procesos y sus relaciones.
    /// </summary>
    public interface IProcessRepository
    {
        /// <summary>
        /// Añade un proceso al soporte de datos.
        /// </summary>
        /// <param name="process">Proceso a añadir.</param>
        void AddProcess(Process process);

        /// <summary>
        /// Obtiene un proceso del soporte de datos a partir de su identificador.
        /// </summary>
        /// <typeparam name="T">Tipo de proceso a obtener</typeparam>
        /// <param name="id">Identificador del proceso.</param>
        /// <returns>Proceso obtenido del soporte de datos; de no existir, <see langword="null"/>.</returns>
        T? GetProcessById<T>(Guid id) where T : Process;

        /// <summary>
        /// Obtiene todos los procesos del soporte de datos.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetAllProcesses<T>() where T : Process;

        /// <summary>
        /// Actualiza el valor de un proceso en el soporte de datos.
        /// </summary>
        /// <param name="process">Instancia con la información a actualizar del proceso.</param>
        void UpdateProcess(Process process);

        /// <summary>
        /// Elimina un proceso del soporte de datos
        /// </summary>
        /// <param name="process">Proceso a eliminar.</param>
        void DeleteProcess(Process process);

        
    }
}
