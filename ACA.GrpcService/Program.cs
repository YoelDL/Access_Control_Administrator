using ACA.Application.Contracts.Processes;
using ACA.Contracts;
using ACA.Contracts.Workers;
using ACA.DataAccess;
using ACA.DataAccess.Contexts;
using ACA.DataAccess.Repositories;
using ACA.Services;
using System.Reflection.Metadata;

namespace ACA.GrpcService
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            // Add services to the container.
            builder.Services.AddGrpc();
            builder.Services.AddAutoMapper(typeof(Program).Assembly);
            builder.Services.AddMediatR(new MediatRServiceConfiguration()
            {
                AutoRegisterRequestProcessors = true,
            }
            .RegisterServicesFromAssemblies(typeof(AssemblyReference).Assembly));

            builder.Services.AddSingleton("Data Source=Data.sqlite");
            builder.Services.AddScoped<AplicationContext>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IOperatorRepository, OperatorRepository>();
            builder.Services.AddScoped<IProcessRepository, ProcessRepository>();
            builder.Services.AddScoped<ISupervisorRepository, SupervisorRepository>();


          

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<OperatorService>();
            app.MapGrpcService<ProcessService>();
            app.MapGrpcService<SupervisorService>();

            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}