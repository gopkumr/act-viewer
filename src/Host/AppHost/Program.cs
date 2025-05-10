using Aspire.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);
builder.AddProject<Projects.Arinco_BicepHub_App>("arinco-bicephub-app");
builder.Build().Run();
