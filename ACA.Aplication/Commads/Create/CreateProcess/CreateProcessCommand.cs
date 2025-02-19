using ACA.Domain.Entities.Processes;
using ACA.Application.Abstract;


namespace ACA.Application.Commads.Create.CreateProcess
{
    public class CreateProcessCommand : ICommand<Process>
    {
        public string ProcessName { get; set; }
        public string ProductName { get; set; }

        public CreateProcessCommand(string processName, string productName)
        {
            ProcessName = processName;
            ProductName = productName;
        }
    }
}
