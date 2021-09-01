# Congree File Processing Service


# Used technologies and dependencies
- [ASP.NET Core] ASP.NET Core 5 Web API 
- [Ef Core] Entity Framework Core 5
- [MediatR] Mediator package for Mediator pattern implementation
- [FluentValidation] FluentValidation for validate request models
- [FluentAssertion] FluentAssertion for assert tests
- [Swashbuckle] Swagger Open API Specification
- [Healtchecks] Healtcheck for check system availability

# Patterns used 
- [Unit of work] - 
- [Repository] -  
- [Mediator] - 
- [CQRS] - 

# CI/CD
- Branching Strategy: GitFlow
- Automatic builds are implemented in Azure Pipelines

# Environments
- Production: https://kopadze.ge/api/swagger
- Heath Check Url: https://kopadze.ge/health


# Configuration
- If you need to test application using EF InMemory Provider, set "UseInMemoryDatabase" property to "True" into appsettings.json
- When multiple request should be processed, default Task threashold is 2. if you want to override this setting, change in appsettings.json file the property "MaxNumOfParallelOperations" to N integer value. 