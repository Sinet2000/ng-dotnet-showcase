# API

## Table Of Contents
- [DB Migrations](#db-migrations)

---

## DB Migrations

### Create New Migration (AppDbContext)

**macOS/Linux:**
```bash
dotnet ef migrations add <MigrationName> --project WebStore.API --startup-project WebStore.API --context AppDbContext --output-dir Migrations/App
```

**Windows (PowerShell/CMD):**
```powershell
dotnet ef migrations add <MigrationName> --project WebStore.API --startup-project WebStore.API --context AppDbContext --output-dir Migrations\App
```

---

### Create New Migration (IdentityDbContext)

**macOS/Linux:**
```bash
dotnet ef migrations add <MigrationName> --project WebStore.API --startup-project WebStore.API --context IdentityDbContext --output-dir Migrations/Identity
```

**Windows (PowerShell/CMD):**
```powershell
dotnet ef migrations add <MigrationName> --project WebStore.API --startup-project WebStore.API --context IdentityDbContext --output-dir Migrations\Identity
```

---

### Update Database

**All Platforms:**
```bash
# Update AppDbContext
dotnet ef database update --project WebStore.API --startup-project WebStore.API --context AppDbContext

# Update IdentityDbContext
dotnet ef database update --project WebStore.API --startup-project WebStore.API --context IdentityDbContext
```

---

### Remove Last Migration

**All Platforms:**
```bash
# Remove from AppDbContext
dotnet ef migrations remove --project WebStore.API --startup-project WebStore.API --context AppDbContext

# Remove from IdentityDbContext
dotnet ef migrations remove --project WebStore.API --startup-project WebStore.API --context IdentityDbContext
```

---

### List Migrations

**All Platforms:**
```bash
# List AppDbContext migrations
dotnet ef migrations list --project WebStore.API --startup-project WebStore.API --context AppDbContext

# List IdentityDbContext migrations
dotnet ef migrations list --project WebStore.API --startup-project WebStore.API --context IdentityDbContext
```

---

> **Note:** Forward slashes (`/`) in `--output-dir` work on all platforms in modern .NET CLI, but backslashes (`\`) are shown for Windows convention.

---