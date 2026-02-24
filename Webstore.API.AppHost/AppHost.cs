var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");
var identityDb = postgres.AddDatabase("webstore-identity");
var mainDb = postgres.AddDatabase("webstore-main");

builder.AddProject<Projects.Webstore_API>("webstore-api")
    .WaitFor(postgres)
    .WithReference(identityDb)
    .WithReference(mainDb);

builder.Build().Run();
