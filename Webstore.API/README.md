# Infrastructure

## Table Of Contents

---

## DB Migrations

### Create New (AppDbContext)
```bash
dotnet ef migrations add <MigrationName> --project WebStore.API --startup-project WebStore.API --context AppDbContext --output-dir Migrations/App
```

### Create New Migration (IdentityDbContext)
```bash
dotnet ef migrations add <MigrationName> --project WebStore.API --startup-project WebStore.API --context IdentityDbContext --output-dir Migrations/Identity
```
