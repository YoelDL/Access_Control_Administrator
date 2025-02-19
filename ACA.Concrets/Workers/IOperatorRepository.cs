using ACA.Domain.Entities.Workers;

namespace ACA.Contracts.Workers
{
    /// <summary>
    /// Describe las funcionalidades necesarias
    /// para dar persistencia a operadores.
    /// </summary>
    public interface IOperatorRepository
    {
        /// <summary>
        /// Añade un operador al soporte de datos.
        /// </summary>
        /// <param name="operador">Cliente a añadir.</param>
        void AddOperator(Operator operador);

        /// <summary>
        /// Obtiene un Operador del soporte de datos a partir de su identificador.
        /// </summary>
        /// <typeparam name="T">Tipo de Operador a obtener.</typeparam>
        /// <param name="id">Identificador del Operador.</param>
        /// <returns>Operador obtenido del soporte de datos; de no existir, <see langword="null"/>.</returns>
        T? GetOperatorById<T>(Guid id) where T: Operator;

        /// <summary>
        /// Obtiene todos los operadores del soporte de datos.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetAllOperators<T>() where T : Operator;

        /// <summary>
        /// Actualiza el valor de un operador en el soporte de datos.
        /// </summary>
        /// <param name="operador">Instancia con la información a actualizar del operador.</param>
        void UpdateOperator(Operator operador);

        /// <summary>
        /// Elimina un operador del soporte de datos
        /// </summary>
        /// <param name="operador">Operador a eliminar.</param>
        void DeleteOperator(Operator operador);
        
    }
}
