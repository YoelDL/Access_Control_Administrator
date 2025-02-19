using ACA.Domain.Entities.Workers;
using ACA.Domain.Entities.Processes;
using ACA.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ACA.Domain.Entities.Types;
using ACA.Domain.Relations;
using Grpc.Net.Client;
using ACA.Application.Contracts.Processes;
using ACA.Contracts.Workers;
using ACA.Contracts;
using ACA.DataAccess.Repositories;
using ACA.DataAccess;

namespace ACA.ConsoleApp
{
    internal class Program
    { 
    static void Main(string[] args)
    {
        Console.WriteLine("Presione una tecla para conectar");
        Console.ReadKey();

        Console.WriteLine("Creando canal y cliente");

        var httpHandler = new HttpClientHandler();
        httpHandler.ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

        var channel = GrpcChannel.ForAddress(
            "http://localhost:5051",  
            new GrpcChannelOptions { HttpHandler = httpHandler });


        if (channel is null)
        {
            Console.WriteLine("No se puede conectar");
            return;
        }

        // Crear clienteslos servicios gRPC
        var operatorClient = new ACA.GrpcProtos.OperatorServiceWeb.OperatorServiceWebClient(channel);
        var processClient = new ACA.GrpcProtos.ProcessServiceWeb.ProcessServiceWebClient(channel);
        var supervisorClient = new ACA.GrpcProtos.SupervisorServiceWeb.SupervisorServiceWebClient(channel);

        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                services.AddDbContext<AplicationContext>(options =>
                {
                    options.UseSqlite("Data Source=AccessControlAdministrator.DB.sqlite");
                });
                services.AddTransient<IAppService, AppService>();

                // Registrar los clientes gRPC
                services.AddSingleton(operatorClient);
                services.AddSingleton(processClient);
                services.AddSingleton(supervisorClient);

                services.AddAutoMapper(typeof(Program).Assembly);
                services.AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssembly(typeof(ACA.Application.AssemblyReference).Assembly);
                });
                services.AddScoped<IOperatorRepository, OperatorRepository>();
                services.AddScoped<ISupervisorRepository, SupervisorRepository>();
                services.AddScoped<IProcessRepository, ProcessRepository>();
                services.AddScoped<IUnitOfWork, UnitOfWork>();
            })
            .Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<AplicationContext>();
                context.Database.EnsureCreated();
                if (context.Database.CanConnect())
                {
                    Console.WriteLine("Conexión a la base de datos exitosa.");
                }
                else
                {
                    Console.WriteLine("No se pudo conectar a la base de datos.");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error al conectar a la base de datos: {ex.Message}");
                return;
            }

            var appService = services.GetRequiredService<IAppService>();
            appService.RunApp();
        }

        Console.WriteLine("Presione una tecla para salir");
        Console.ReadKey();
        channel.ShutdownAsync().Wait();
    }
}


public interface IAppService
{
    void RunApp();
}

public class AppService : IAppService
{
    private readonly AplicationContext _context;

    public AppService(AplicationContext context)
    {
        _context = context;
    }

    public void RunApp()
    {
        while (true)
        {
            Console.WriteLine("\nElige una opción:\n");
            Console.WriteLine("1. Crear");
            Console.WriteLine("2. Obtener ");
            Console.WriteLine("3. Listado");
            Console.WriteLine("4. Actualizar");
            Console.WriteLine("5. Eliminar");
            Console.WriteLine("6. Gestion");
            Console.WriteLine("\n7. Salir");

            Console.Write("\nOpción: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateEntity();
                    break;
                case "2":
                    GetEntity();
                    break;
                case "3":
                    ListEntities();
                    break;
                case "4":
                    UpdateEntity();
                    break;
                case "5":
                    DeleteEntity();
                    break;
                case "6":
                    ManageAssignments();
                    break;
                case "7":
                    Console.WriteLine("Saliendo...");
                    return;
                default:
                    Console.WriteLine("Opción inválida.");
                    break;
            }
        }
    }
    private void CreateEntity()  // Apartado para crear una entidad en la base de datos
    {
        Console.WriteLine("\nElige qué tipo de entidad crear:\n");
        Console.WriteLine("1. Operador");
        Console.WriteLine("2. Supervisor");
        Console.WriteLine("3. Proceso");
        Console.WriteLine("\n4. Menu principal");

        Console.Write("\nOpción: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                CreateOperator();
                break;
            case "2":
                CreateSupervisor();
                break;
            case "3":
                CreateProcess();
                break;
            case "4":
                Console.WriteLine("Volviendo al menu principal...");
                break;
            default:
                Console.WriteLine("Opción inválida.");
                break;
        }
    }

    private void CreateOperator()
    {
        Console.WriteLine("\nCreando un nuevo Operador...");

        Console.Write("Nombre del Operador: ");
        string operatorName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(operatorName))
        {
            Console.WriteLine("El nombre del Operador es obligatorio.");
            return;
        }

        Console.Write("Número de Identidad (CI): ");
        string ci = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(ci))
        {
            Console.WriteLine("El número de CI es obligatorio.");
            return;
        }

        // Validación de la longitud del CI
        if (ci.Length != 11 || !ci.All(char.IsDigit))
        {
            Console.WriteLine("Número de CI inválido. Debe tener exactamente 11 dígitos numéricos.");
            return;
        }

        // Validación de CI duplicado en la base de datos
        if (_context.Operators.Any(o => o.CI == ci))
        {
            Console.WriteLine($"Ya existe un Operador con el CI '{ci}'.");
            return;
        }

        Console.WriteLine("Nivel de Escolaridad:\n");
        Console.WriteLine("0. Medium");
        Console.WriteLine("1. High");
        Console.WriteLine("2. MediumTecnic");
        Console.WriteLine("3. University");
        Console.Write("\nOpción: ");

        if (int.TryParse(Console.ReadLine(), out int schoolLevelValue))
        {
            if (Enum.IsDefined(typeof(SchoolLevel), schoolLevelValue))
            {
                SchoolLevel schoolLevel = (SchoolLevel)schoolLevelValue;
                Guid operatorId = Guid.NewGuid(); // Generar ID aleatorio para el operador

                var newOperator = new Operator
                (
                    operatorName,
                    ci,
                    operatorId, // Usar el Guid generado
                    schoolLevel,
                    null, // Inicialmente sin supervisor
                    new List<Process_Operator>()
                );

                _context.Operators.Add(newOperator);
                _context.SaveChanges();
                Console.WriteLine($"Operador '{operatorName}' creado con éxito.");
            }
            else
            {
                Console.WriteLine("Valor de Nivel de Escolaridad fuera de rango.");
            }
        }
        else
        {
            Console.WriteLine("Nivel de Escolaridad inválido.");
        }
    }

    private void CreateSupervisor()
    {
        Console.WriteLine("\nCreando un nuevo Supervisor...");

        Console.Write("Nombre del Supervisor: ");
        string supervisorName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(supervisorName))
        {
            Console.WriteLine("El nombre del Supervisor es obligatorio.");
            return;
        }

        Console.Write("Número de Identidad (CI): ");
        string ci = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(ci))
        {
            Console.WriteLine("El número de CI es obligatorio.");
            return;
        }

        // Validación de la longitud del CI
        if (ci.Length != 11 || !ci.All(char.IsDigit))
        {
            Console.WriteLine("Número de CI inválido. Debe tener exactamente 11 dígitos numéricos.");
            return;
        }

        // Validación de CI duplicado en la base de datos
        if (_context.Supervisors.Any(s => s.CI == ci))
        {
            Console.WriteLine($"Ya existe un Supervisor con el CI '{ci}'.");
            return;
        }

        Console.WriteLine("Nivel de Escolaridad:\n");
        Console.WriteLine("0. Medium");
        Console.WriteLine("1. High");
        Console.WriteLine("2. MediumTecnic");
        Console.WriteLine("3. University");
        Console.Write("\nOpción: ");

        if (int.TryParse(Console.ReadLine(), out int schoolLevelValue))
        {
            if (Enum.IsDefined(typeof(SchoolLevel), schoolLevelValue))
            {
                SchoolLevel schoolLevel = (SchoolLevel)schoolLevelValue;
                Guid supervisorId = Guid.NewGuid();

                var newSupervisor = new Supervisor
                (
                    supervisorName,
                    ci,
                    schoolLevel,
                    supervisorId,
                    new List<Operator>(),
                    new List<Process_Supervisor>()
                );

                _context.Supervisors.Add(newSupervisor);
                _context.SaveChanges();
                Console.WriteLine($"Supervisor '{supervisorName}' creado con éxito.");
            }
            else
            {
                Console.WriteLine("Valor de Nivel de Escolaridad fuera de rango.");
            }
        }
        else
        {
            Console.WriteLine("Nivel de Escolaridad inválido.");
        }
    }


    private void CreateProcess()
    {
        Console.WriteLine("\nCreando un nuevo Proceso...");

        Console.Write("Nombre del Proceso: ");
        string processName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(processName))
        {
            Console.WriteLine("El nombre del Proceso es obligatorio.");
            return;
        }

        Console.Write("Nombre del Producto: ");
        string productName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(productName))
        {
            Console.WriteLine("El nombre del Producto es obligatorio.");
            return;
        }

        Guid processId = Guid.NewGuid();
        var newProcess = new Process
        (
            processName,
            productName,
            processId,
            new List<Process_Operator>(),
            new List<Process_Supervisor>()
        );

        _context.Processes.Add(newProcess);
        _context.SaveChanges();
        Console.WriteLine($"Proceso '{processName}' creado con éxito.");
    }


    private void GetEntity() // Apartado para obtener una sola entidad de la base de datos
    {
        Console.WriteLine("\nElige qué tipo de entidad desea obtener:\n");
        Console.WriteLine("1. Un Operador");
        Console.WriteLine("2. Un Supervisor");
        Console.WriteLine("3. Un Proceso");
        Console.WriteLine("\n4. Menu principal");

        Console.Write("\nOpción: ");
        string choice = Console.ReadLine();


        switch (choice)
        {
            case "1":
                GetOperator();
                break;
            case "2":
                GetSupervisor();
                break;
            case "3":
                GetProcess();
                break;
            case "4":
                Console.WriteLine("Volviendo al menu principal...");
                break;
            default:
                Console.WriteLine("Opción inválida.");
                break;
        }   
    }

    private void GetOperator()
    {
        Console.WriteLine("\nObteniendo un Operador...");
        Console.Write("Ingrese el ID del Operador a obtener: ");
        if (Guid.TryParse(Console.ReadLine(), out var operatorId))
        {
            // Usar Include para cargar los procesos relacionados
            var operatorToGet = _context.Operators
                .Include(o => o.Processes_Operators) 
                .FirstOrDefault(o => o.Id == operatorId);

            if (operatorToGet != null)
            {
                Console.WriteLine($"\nDatos del Operador:");
                Console.WriteLine($"ID: {operatorToGet.Id}");
                Console.WriteLine($"Nombre: {operatorToGet.Operator_Name}");
                Console.WriteLine($"CI: {operatorToGet.CI}");
                Console.WriteLine($"Nivel de Escolaridad: {operatorToGet.schoolLevel}");
                Console.WriteLine($"Supervisor Asignado: {operatorToGet.Asigned_Supervisor?.Supervisor_Name ?? "Ninguno"}");

                // Mostrar los procesos asignados
                if (operatorToGet.Processes_Operators != null && operatorToGet.Processes_Operators.Any())
                {
                    Console.WriteLine("Procesos Asignados:");
                    foreach (var process in operatorToGet.Processes_Operators)
                    {
                        Console.WriteLine($"- ID: {process.ProcessId}");
                    }
                }
                else
                {
                    Console.WriteLine("No tiene procesos asignados.");
                }
            }
            else
            {
                Console.WriteLine($"No se encontró un Operador con el ID '{operatorId}'.");
            }
        }
        else
        {
            Console.WriteLine("ID de Operador inválido.");
        }
    }



    private void GetSupervisor()
    {
        Console.WriteLine("\nObteniendo un Supervisor...");
        Console.Write("Ingrese el ID del Supervisor a obtener: ");
        if (Guid.TryParse(Console.ReadLine(), out var supervisorId))
        {
            // Usar Include para cargar los procesos y operadores relacionados
            var supervisorToGet = _context.Supervisors
                .Include(s => s.Processes_Supervisors)
                    .ThenInclude(ps => ps.Process)
                .Include(s => s.Operators)
                .FirstOrDefault(s => s.Id == supervisorId);

            if (supervisorToGet != null)
            {
                Console.WriteLine($"\nDatos del Supervisor:");
                Console.WriteLine($"ID: {supervisorToGet.Id}");
                Console.WriteLine($"Nombre: {supervisorToGet.Supervisor_Name}");
                Console.WriteLine($"CI: {supervisorToGet.CI}");
                Console.WriteLine($"Nivel de Escolaridad: {supervisorToGet.schoolLevel}");

                // Mostrar los procesos asignados
                if (supervisorToGet.Processes_Supervisors != null && supervisorToGet.Processes_Supervisors.Any())
                {
                    Console.WriteLine("Procesos Asignados:");
                    foreach (var processSupervisor in supervisorToGet.Processes_Supervisors)
                    {
                        var process = processSupervisor.Process;
                        Console.WriteLine($"- ID: {process.Id}, Nombre: {process.ProcessName}");
                    }
                }
                else
                {
                    Console.WriteLine("No tiene procesos asignados.");
                }

                // Mostrar los operadores asignados
                if (supervisorToGet.Operators != null && supervisorToGet.Operators.Any())
                {
                    Console.WriteLine("Operadores Asignados:");
                    foreach (var op in supervisorToGet.Operators)
                    {
                        Console.WriteLine($"- ID: {op.Id}, Nombre: {op.Operator_Name}");
                    }
                }
                else
                {
                    Console.WriteLine("No tiene operadores asignados.");
                }
            }
            else
            {
                Console.WriteLine($"No se encontró un Supervisor con el ID '{supervisorId}'.");
            }
        }
        else
        {
            Console.WriteLine("ID de Supervisor inválido.");
        }
    }




    private void GetProcess() 
    {
        Console.WriteLine("\nObteniendo un Proceso...");
        Console.Write("Ingrese el ID del Proceso a obtener: ");
        if (Guid.TryParse(Console.ReadLine(), out var processId))
        {
            var processToGet = _context.Processes.Find(processId);
            if (processToGet != null)
            {
                Console.WriteLine($"\nDatos del Proceso:");
                Console.WriteLine($"ID: {processToGet.Id}");
                Console.WriteLine($"Nombre: {processToGet.ProcessName}");
                Console.WriteLine($"Producto: {processToGet.ProductName}");
            }
            else
            {
                Console.WriteLine($"No se encontró un Proceso con el ID '{processId}'.");
            }
        }
        else
        {
            Console.WriteLine("ID de Proceso inválido.");
        }
    }


    private void ListEntities()   // Apartado para obtener una lista de todas las entidades en la base de datos
    {
        Console.WriteLine("\nElige cual lista desea obtener:\n");
        Console.WriteLine("1. Operadores");
        Console.WriteLine("2. Supervisores");
        Console.WriteLine("3. Procesos");
        Console.WriteLine("\n4. Menu principal");

        Console.Write("Opción: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                ListOperators();
                break;
            case "2":
                ListSupervisors();
                break;
            case "3":
                ListProcesses();
                break;
            case "4":
                Console.WriteLine("Volviendo al menu principal...");
                break;
            default:
                Console.WriteLine("Opción inválida.");
                break;
        }

    }

    private void ListOperators()
    {
        Console.WriteLine("\nListando Operadores...");
        foreach (var op in _context.Operators)
        {
            Console.WriteLine($"ID: {op.Id}, Nombre: {op.Operator_Name}, CI: {op.CI}, Supervisor: {op.Asigned_Supervisor?.Supervisor_Name ?? "Ninguno"}");
        }
    }

    private void ListSupervisors()
    {
        Console.WriteLine("\nListando Supervisores...");
        foreach (var sup in _context.Supervisors)
        {
            Console.WriteLine($"ID: {sup.Id}, Nombre: {sup.Supervisor_Name}, CI: {sup.CI}");
        }
    }

    private void ListProcesses()
    {
        Console.WriteLine("\nListando Procesos...");
        foreach (var proc in _context.Processes)
        {
            Console.WriteLine($"ID: {proc.Id}, Nombre: {proc.ProcessName}, Producto: {proc.ProductName}");
        }
    }
    private void UpdateEntity()
    {
        Console.WriteLine("\nElige qué tipo de entidad actualizar:\n");
        Console.WriteLine("1. Operador");
        Console.WriteLine("2. Supervisor");
        Console.WriteLine("3. Proceso");
        Console.WriteLine("\n4. Menu principal");

        Console.Write("Opción: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                UpdateOperator();
                break;
            case "2":
                UpdateSupervisor();
                break;
            case "3":
                UpdateProcess();
                break;
            case "4":
                Console.WriteLine("Volviendo al menu principal...");
                break;
            default:
                Console.WriteLine("Opción inválida.");
                break;
        }
    }

    private void UpdateOperator()
    {
        Console.WriteLine("\nActualizando un Operador...");
        Console.Write("Ingrese el ID del Operador a actualizar: ");
        if (Guid.TryParse(Console.ReadLine(), out var operatorId))
        {
            var operatorToUpdate = _context.Operators.Find(operatorId);
            if (operatorToUpdate != null)
            {
                Console.Write("Nuevo Nombre del Operador (dejar en blanco para no cambiar): ");
                string newName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newName))
                {
                    operatorToUpdate.Operator_Name = newName;
                }

                Console.Write("Nuevo Número de Identidad (CI) (dejar en blanco para no cambiar): ");
                string newCi = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newCi))
                {
                    operatorToUpdate.CI = newCi;
                }

                Console.WriteLine("Nuevo Nivel de Escolaridad (dejar en blanco para no cambiar):\n");
                Console.WriteLine("0. Medium");
                Console.WriteLine("1. High");
                Console.WriteLine("2. MediumTecnic");
                Console.WriteLine("3. University");
                Console.Write("\nOpción: ");
                string schoolLevelInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(schoolLevelInput) && Enum.TryParse<SchoolLevel>(schoolLevelInput, out var newSchoolLevel))
                {
                    operatorToUpdate.schoolLevel = newSchoolLevel;
                }

                _context.SaveChanges();
                Console.WriteLine($"Operador con ID '{operatorId}' actualizado con éxito.");
            }
            else
            {
                Console.WriteLine($"No se encontró un Operador con el ID '{operatorId}'.");
            }
        }
        else
        {
            Console.WriteLine("ID de Operador inválido.");
        }
    }

    private void UpdateSupervisor()
    {
        Console.WriteLine("\nActualizando un Supervisor...");
        Console.Write("Ingrese el ID del Supervisor a actualizar: ");
        if (Guid.TryParse(Console.ReadLine(), out var supervisorId))
        {
            var supervisorToUpdate = _context.Supervisors.Find(supervisorId);
            if (supervisorToUpdate != null)
            {
                Console.Write("Nuevo Nombre del Supervisor (dejar en blanco para no cambiar): ");
                string newName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newName))
                {
                    supervisorToUpdate.Supervisor_Name = newName;
                }

                Console.Write("Nuevo Número de Identidad (CI) (dejar en blanco para no cambiar): ");
                string newCi = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newCi))
                {
                    supervisorToUpdate.CI = newCi;
                }

                Console.WriteLine("Nuevo Nivel de Escolaridad (dejar en blanco para no cambiar):\n");
                Console.WriteLine("0. Medium");
                Console.WriteLine("1. High");
                Console.WriteLine("2. MediumTecnic");
                Console.WriteLine("3. University");
                Console.Write("\nOpción: ");
                string schoolLevelInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(schoolLevelInput) && Enum.TryParse<SchoolLevel>(schoolLevelInput, out var newSchoolLevel))
                {
                    supervisorToUpdate.schoolLevel = newSchoolLevel;
                }

                _context.SaveChanges();
                Console.WriteLine($"Supervisor con ID '{supervisorId}' actualizado con éxito.");
            }
            else
            {
                Console.WriteLine($"No se encontró un Supervisor con el ID '{supervisorId}'.");
            }
        }
        else
        {
            Console.WriteLine("ID de Supervisor inválido.");
        }
    }

    private void UpdateProcess()
    {
        Console.WriteLine("\nActualizando un Proceso...");
        Console.Write("Ingrese el ID del Proceso a actualizar: ");
        if (Guid.TryParse(Console.ReadLine(), out var processId))
        {
            var processToUpdate = _context.Processes.Find(processId);
            if (processToUpdate != null)
            {
                Console.Write("Nuevo Nombre del Proceso (dejar en blanco para no cambiar): ");
                string newName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newName))
                {
                    processToUpdate.ProcessName = newName;
                }

                Console.Write("Nuevo Nombre del Producto (dejar en blanco para no cambiar): ");
                string newProduct = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newProduct))
                {
                    processToUpdate.ProductName = newProduct;
                }

                _context.SaveChanges();
                Console.WriteLine($"Proceso con ID '{processId}' actualizado con éxito.");
            }
            else
            {
                Console.WriteLine($"No se encontró un Proceso con el ID '{processId}'.");
            }
        }
        else
        {
            Console.WriteLine("ID de Proceso inválido.");
        }
    }
    private void DeleteEntity() // Apartado para eliminar una entidad de la base de datos
    {
        Console.WriteLine("\nElige qué tipo de entidad eliminar:\n");
        Console.WriteLine("1. Operador");
        Console.WriteLine("2. Supervisor");
        Console.WriteLine("3. Proceso");
        Console.WriteLine("\n4. Menu principal");

        Console.Write("\nOpción: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                DeleteOperator();
                break;
            case "2":
                DeleteSupervisor();
                break;
            case "3":
                DeleteProcess();
                break;
            case "4":
                Console.WriteLine("Volviendo al menu principal...");
                break;
            default:
                Console.WriteLine("Opción inválida.");
                break;
        }
    }

    private void DeleteOperator()
    {
        Console.WriteLine("\nEliminando un Operador...");
        Console.Write("Ingrese el ID del Operador a eliminar: ");
        if (Guid.TryParse(Console.ReadLine(), out var operatorId))
        {
            var operatorToDelete = _context.Operators.Find(operatorId);
            if (operatorToDelete != null)
            {
                _context.Operators.Remove(operatorToDelete);
                _context.SaveChanges();
                Console.WriteLine($"Operador con ID '{operatorId}' eliminado con éxito.");
            }
            else
            {
                Console.WriteLine($"No se encontró un Operador con el ID '{operatorId}'.");
            }
        }
        else
        {
            Console.WriteLine("ID de Operador inválido.");
        }
    }

    private void DeleteSupervisor()
    {
        Console.WriteLine("\nEliminando un Supervisor...");
        Console.Write("Ingrese el ID del Supervisor a eliminar: ");
        if (Guid.TryParse(Console.ReadLine(), out var supervisorId))
        {
            var supervisorToDelete = _context.Supervisors.Find(supervisorId);
            if (supervisorToDelete != null)
            {
                _context.Supervisors.Remove(supervisorToDelete);
                _context.SaveChanges();
                Console.WriteLine($"Supervisor con ID '{supervisorId}' eliminado con éxito.");
            }
            else
            {
                Console.WriteLine($"No se encontró un Supervisor con el ID '{supervisorId}'.");
            }
        }
        else
        {
            Console.WriteLine("ID de Supervisor inválido.");
        }
    }

    private void DeleteProcess()
    {
        Console.WriteLine("\nEliminando un Proceso...");
        Console.Write("Ingrese el ID del Proceso a eliminar: ");
        if (Guid.TryParse(Console.ReadLine(), out var processId))
        {
            var processToDelete = _context.Processes.Find(processId);
            if (processToDelete != null)
            {
                _context.Processes.Remove(processToDelete);
                _context.SaveChanges();
                Console.WriteLine($"Proceso con ID '{processId}' eliminado con éxito.");
            }
            else
            {
                Console.WriteLine($"No se encontró un Proceso con el ID '{processId}'.");
            }
        }
        else
        {
            Console.WriteLine("ID de Proceso inválido.");
        }
    }
    private void ManageAssignments()  // Apartado para asignaciones, aqui se ven refleadas las relacion, Nota, al asignarle un supervisor 
    {                                 // a un operador se refleja automticamente dicho operador en el supervisor
        Console.WriteLine("\nElige qué tipo de gestión desea realizar:");
        Console.WriteLine("\n----Crear nuevas relaciones----\n");
        Console.WriteLine("1. Asignar Supervisor a Operador");
        Console.WriteLine("2. Agregar Proceso a Operador");
        Console.WriteLine("3. Agregar Proceso a Supervisor");
        Console.WriteLine("\n----Eliminar relaciones----\n");
        Console.WriteLine("4. Eliminar un Supervisor de un Operador");
        Console.WriteLine("3. Desagregar un Proceso de un Supervisor");
        Console.WriteLine("3. Desagregar un Proceso de un Operador");
        Console.WriteLine("\n7. Menu principal");
        Console.Write("\nOpción: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                AssignSupervisorToOperator();
                break;
            case "2":
                AddProcessToOperator();
                break;
            case "3":
                AddProcessToSupervisor();
                break;
            case "4":
                UnassignSupervisorFromOperator();
                break;
            case "5":
                RemoveProcessFromSupervisor();
                break;
            case "6":
                RemoveProcessFromOperator();
                break;
            case "7":
                Console.WriteLine("Volviendo al menu principal...");
                break;
            default:
                Console.WriteLine("Opción inválida.");
                break;
        }
    }

    private void AssignSupervisorToOperator()
    {
        Console.WriteLine("\nAsignando Supervisor a Operador...");

        Console.Write("Ingrese el ID del Operador: ");
        if (!Guid.TryParse(Console.ReadLine(), out var operatorId))
        {
            Console.WriteLine("ID de Operador inválido.");
            return;
        }

        var operador = _context.Operators.Find(operatorId);
        if (operador == null)
        {
            Console.WriteLine($"No se encontró un Operador con el ID '{operatorId}'.");
            return;
        }

        Console.Write("Ingrese el ID del Supervisor: ");
        if (!Guid.TryParse(Console.ReadLine(), out var supervisorId))
        {
            Console.WriteLine("ID de Supervisor inválido.");
            return;
        }

        var supervisor = _context.Supervisors.Find(supervisorId);
        if (supervisor == null)
        {
            Console.WriteLine($"No se encontró un Supervisor con el ID '{supervisorId}'.");
            return;
        }

        operador.Asigned_Supervisor = supervisor;
        _context.SaveChanges();
        Console.WriteLine($"Supervisor con ID '{supervisorId}' asignado al Operador con ID '{operatorId}' con éxito.");
    }

    private void AddProcessToOperator()
    {
        Console.WriteLine("\nAgregando Proceso a Operador...");

        Console.Write("Ingrese el ID del Operador: ");
        if (!Guid.TryParse(Console.ReadLine(), out var operatorId))
        {
            Console.WriteLine("ID de Operador inválido.");
            return;
        }

        var operador = _context.Operators.Find(operatorId);
        if (operador == null)
        {
            Console.WriteLine($"No se encontró un Operador con el ID '{operatorId}'.");
            return;
        }

        Console.Write("Ingrese el ID del Proceso: ");
        if (!Guid.TryParse(Console.ReadLine(), out var processId))
        {
            Console.WriteLine("ID de Proceso inválido.");
            return;
        }

        var process = _context.Processes.Find(processId);
        if (process == null)
        {
            Console.WriteLine($"No se encontró un Proceso con el ID '{processId}'.");
            return;
        }

        var processOperator = new Process_Operator
        {
            ProcessId = processId,
            OperatorId = operatorId
        };

        operador.Processes_Operators.Add(processOperator);
        _context.SaveChanges();
        Console.WriteLine($"Proceso con ID '{processId}' agregado al Operador con ID '{operatorId}' con éxito.");
    }

    private void AddProcessToSupervisor()
    {
        Console.WriteLine("\nAgregando Proceso a Supervisor...");

        Console.Write("Ingrese el ID del Supervisor: ");
        if (!Guid.TryParse(Console.ReadLine(), out var supervisorId))
        {
            Console.WriteLine("ID de Supervisor inválido.");
            return;
        }

        var supervisor = _context.Supervisors.Find(supervisorId);
        if (supervisor == null)
        {
            Console.WriteLine($"No se encontró un Supervisor con el ID '{supervisorId}'.");
            return;
        }

        Console.Write("Ingrese el ID del Proceso: ");
        if (!Guid.TryParse(Console.ReadLine(), out var processId))
        {
            Console.WriteLine("ID de Proceso inválido.");
            return;
        }

        var process = _context.Processes.Find(processId);
        if (process == null)
        {
            Console.WriteLine($"No se encontró un Proceso con el ID '{processId}'.");
            return;
        }

        var processSupervisor = new Process_Supervisor
        {
            ProcessId = processId,
            SupervisorId = supervisorId
        };

        supervisor.Processes_Supervisors.Add(processSupervisor);
        _context.SaveChanges();
        Console.WriteLine($"Proceso con ID '{processId}' agregado al Supervisor con ID '{supervisorId}' con éxito.");
    }

    private void UnassignSupervisorFromOperator()
    {
        Console.WriteLine("\nDesasignando Supervisor de Operador...");

        Console.Write("Ingrese el ID del Operador: ");
        if (!Guid.TryParse(Console.ReadLine(), out var operatorId))
        {
            Console.WriteLine("ID de Operador inválido.");
            return;
        }

        var operador = _context.Operators.Include(o => o.Asigned_Supervisor).FirstOrDefault(o => o.Id == operatorId);
        if (operador == null)
        {
            Console.WriteLine($"No se encontró un Operador con el ID '{operatorId}'.");
            return;
        }

        operador.Asigned_Supervisor = null; // Desasignar el supervisor
        _context.SaveChanges();
        Console.WriteLine($"Supervisor desasignado del Operador con ID '{operatorId}' con éxito.");
    }

    private void RemoveProcessFromSupervisor()
    {
        Console.WriteLine("\nEliminando Proceso de Supervisor...");

        Console.Write("Ingrese el ID del Supervisor: ");
        if (!Guid.TryParse(Console.ReadLine(), out var supervisorId))
        {
            Console.WriteLine("ID de Supervisor inválido.");
            return;
        }

        var supervisor = _context.Supervisors.Include(s => s.Processes_Supervisors).FirstOrDefault(s => s.Id == supervisorId);
        if (supervisor == null)
        {
            Console.WriteLine($"No se encontró un Supervisor con el ID '{supervisorId}'.");
            return;
        }

        Console.Write("Ingrese el ID del Proceso a eliminar: ");
        if (!Guid.TryParse(Console.ReadLine(), out var processId))
        {
            Console.WriteLine("ID de Proceso inválido.");
            return;
        }

        var processSupervisor = supervisor.Processes_Supervisors.FirstOrDefault(ps => ps.ProcessId == processId);
        if (processSupervisor == null)
        {
            Console.WriteLine($"No se encontró la relación entre el Proceso '{processId}' y el Supervisor '{supervisorId}'.");
            return;
        }

        _context.Set<Process_Supervisor>().Remove(processSupervisor); // Eliminar la relación
        _context.SaveChanges();
        Console.WriteLine($"Proceso con ID '{processId}' eliminado del Supervisor con ID '{supervisorId}' con éxito.");
    }

    private void RemoveProcessFromOperator()
    {
        Console.WriteLine("\nEliminando Proceso de Operador...");

        Console.Write("Ingrese el ID del Operador: ");
        if (!Guid.TryParse(Console.ReadLine(), out var operatorId))
        {
            Console.WriteLine("ID de Operador inválido.");
            return;
        }

        var operador = _context.Operators.Include(o => o.Processes_Operators).FirstOrDefault(o => o.Id == operatorId);
        if (operador == null)
        {
            Console.WriteLine($"No se encontró un Operador con el ID '{operatorId}'.");
            return;
        }

        Console.Write("Ingrese el ID del Proceso a eliminar: ");
        if (!Guid.TryParse(Console.ReadLine(), out var processId))
        {
            Console.WriteLine("ID de Proceso inválido.");
            return;
        }

        var processOperator = operador.Processes_Operators.FirstOrDefault(po => po.ProcessId == processId);
        if (processOperator == null)
        {
            Console.WriteLine($"No se encontró la relación entre el Proceso '{processId}' y el Operador '{operatorId}'.");
            return;
        }

        _context.Set<Process_Operator>().Remove(processOperator); // Eliminar la relación
        _context.SaveChanges();
        Console.WriteLine($"Proceso con ID '{processId}' eliminado del Operador con ID '{operatorId}' con éxito.");
    }


}
    }