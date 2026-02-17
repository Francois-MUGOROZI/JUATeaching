# Clean Architecture Folder Structure

```markdown
CRMDemo/ ├── src/ │ ├── Core/ │ │ ├── Application/ # Application layer (use
cases, interfaces) │ │ └── Domain/ # Domain layer (entities, value objects,
aggregates) │ ├── Infrastructure/ # Infrastructure layer (data access, external
services) │ └── Web/ # Presentation layer (UI) │ └── API/ # Presentation layer
(UI) ├── tests/ # Tests for each layer ├── docs/ # Documentation ├── scripts/ #
Scripts and automation ├── .gitignore ├── README.md └── CRMDemo.sln
```

## Commands

- Add Migration:
  `dotnet ef migrations add Initial --project ../Infrastructure/Infrastructure.csproj`
