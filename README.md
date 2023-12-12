
# CamAI Capstone Project.

## Before starting.
Project currently uses .NET 8 ([Download at Microsoft's page](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)).  
On terminal and move to project path:
```
../CamAI-backend/CamAISolution
```
Run command to restore dependencies and tools:
```
dotnet restore
```
Then run to build the project:
```
dotnet build
```

## Project Overview.
- CamAI-backend
    - CamAISolution
        - src
            - Core.*
            - Host.*
            - Infrastructure.*
        - test
    - Infrastructure
    - .gitignore
    - Directory.Packages.props

All version of dependencies are defined in `Directory.Packages.props`.

### CamAISolution Part:

#### src:
***
##### Core:
This namespace will implement business logic of the project and **won't accept any dependencies or 3rd parties**. Includes:
- Domain
    - Entites
    - Interfaces
    - Models
    - Utilities
    - ...
- Application
    - Implements
    - Exceptions
    - Specifications
    - ...

##### Infrastructure:
In case we need third party, this namespace will implement code from third parties like `EF, Database, Jwt...` It must implement the interfaces that exposed by the `Core.Domain.Interfaces`.

##### Host:
Defined endpoint of the project.
- Host.Something.API
- Host.Something.BlazorWeb
- ...