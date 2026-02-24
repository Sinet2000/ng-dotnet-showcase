var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");
var postgresdb = postgres.AddDatabase("postgresdb");

builder.AddProject<Projects.Webstore_API>("webstore-api")
    .WaitFor(postgresdb)
    .WithReference(postgresdb);

builder.Build().Run();
