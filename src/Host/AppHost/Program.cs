using Aspire.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);
       

builder.AddProject<Projects.ACRViewer_BlazorServer>("acrviewer-blazorserver");
       

builder.Build().Run();
