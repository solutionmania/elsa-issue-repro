using Elsa.EntityFrameworkCore.Extensions;
using Elsa.EntityFrameworkCore.Modules.Runtime;
using Elsa.Extensions;
using Elsa.Workflows.Core.Activities;
using Elsa.Workflows.Core.Contracts;
using Elsa.Workflows.Core.Services;
using Microsoft.Extensions.DependencyInjection;

// Setup service container.
var services = new ServiceCollection();

// Add Elsa services to the container.
services.AddElsa(elsa =>
{
    elsa.UseWorkflowRuntime(runtime =>
    {
        // Store detils of running workflows in SQL Server
        runtime.UseEntityFrameworkCore(ef =>
        {
            ef.UseSqlServer("Server=localhost;Database=ElsaPlayground;Integrated Security=True;TrustServerCertificate=True;");
        });
    });
});

// Build the service container.
var serviceProvider = services.BuildServiceProvider();

// Define a simple workflow that writes a message to the console.
var workflow = new Sequence
{
    Activities =
    {
        new WriteLine("Hello World!"),
        new WriteLine("Goodbye cruel world...")
    }
};

// Resolve a workflow runner to execute the workflow.
var workflowRunner = serviceProvider.GetRequiredService<IWorkflowRunner>();

// Execute the workflow.
await workflowRunner.RunAsync(workflow);

