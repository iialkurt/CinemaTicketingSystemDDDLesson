# .NET 10.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that a .NET 10.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 10.0 upgrade.
3. Upgrade CinemaTicketingSystem.SharedKernel.csproj
4. Upgrade CinemaTicketingSystem.Application.Contracts.csproj
5. Upgrade CinemaTicketingSystem.Domain.csproj
6. Upgrade CinemaTicketingSystem.Infrastructure.Caching.csproj
7. Upgrade CinemaTicketingSystem.Infrastructure.Messaging.csproj
8. Upgrade CinemaTicketingSystem.Presentation.API.csproj
9. Upgrade CinemaTicketingSystem.Application.csproj
10. Upgrade CinemaTicketingSystem.Infrastructure.Persistence.csproj
11. Upgrade CinemaTicketingSystem.Infrastructure.Authentication.csproj
12. Upgrade CinemaTicketingSystem.WebApi.Host.csproj
13. Upgrade CinemaTicketingSystem.Application.Test.csproj
14. Upgrade CinemaTicketingSystem.Domain.Test.csproj
15. Upgrade CinemaTicketingSystem.Infrastructure.DbMigrator.csproj
16. Run unit tests to validate upgrade in the projects listed below:
    - CinemaTicketingSystem.Domain.Test.csproj
    - CinemaTicketingSystem.Application.Test.csproj

## Settings

This section contains settings and data used by execution steps.

### Aggregate NuGet packages modifications across all projects

NuGet packages used across all selected projects or their dependencies that need version update in projects that reference them.

| Package Name                                          | Current Version | New Version | Description                           |
|:------------------------------------------------------|:---------------:|:-----------:|:--------------------------------------|
| Microsoft.AspNetCore.Authentication.JwtBearer         | 9.0.7           | 10.0.0      | Recommended for .NET 10.0             |
| Microsoft.AspNetCore.Identity.EntityFrameworkCore     | 9.0.7           | 10.0.0      | Recommended for .NET 10.0             |
| Microsoft.AspNetCore.Mvc.Testing                      | 9.0.9           | 10.0.0      | Recommended for .NET 10.0             |
| Microsoft.AspNetCore.OpenApi                          | 9.0.5           | 10.0.0      | Recommended for .NET 10.0             |
| Microsoft.EntityFrameworkCore                         | 9.0.7           | 10.0.0      | Recommended for .NET 10.0             |
| Microsoft.EntityFrameworkCore.Design                  | 9.0.7           | 10.0.0      | Recommended for .NET 10.0             |
| Microsoft.EntityFrameworkCore.Proxies                 | 9.0.7           | 10.0.0      | Recommended for .NET 10.0             |
| Microsoft.EntityFrameworkCore.SqlServer               | 9.0.7           | 10.0.0      | Recommended for .NET 10.0             |
| Microsoft.EntityFrameworkCore.Tools                   | 9.0.7           | 10.0.0      | Recommended for .NET 10.0             |
| Microsoft.Extensions.Hosting                          | 9.0.7           | 10.0.0      | Recommended for .NET 10.0             |

### Project upgrade details

This section contains details about each project upgrade and modifications that need to be done in the project.

#### CinemaTicketingSystem.SharedKernel.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`

#### CinemaTicketingSystem.Application.Contracts.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`

#### CinemaTicketingSystem.Domain.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`

#### CinemaTicketingSystem.Infrastructure.Caching.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`

#### CinemaTicketingSystem.Infrastructure.Messaging.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`

#### CinemaTicketingSystem.Presentation.API.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`

#### CinemaTicketingSystem.Application.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`

#### CinemaTicketingSystem.Infrastructure.Persistence.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`

NuGet packages changes:
  - Microsoft.AspNetCore.Identity.EntityFrameworkCore should be updated from `9.0.7` to `10.0.0` (*recommended for .NET 10.0*)
  - Microsoft.EntityFrameworkCore should be updated from `9.0.7` to `10.0.0` (*recommended for .NET 10.0*)
  - Microsoft.EntityFrameworkCore.Proxies should be updated from `9.0.7` to `10.0.0` (*recommended for .NET 10.0*)
  - Microsoft.EntityFrameworkCore.SqlServer should be updated from `9.0.7` to `10.0.0` (*recommended for .NET 10.0*)
  - Microsoft.EntityFrameworkCore.Tools should be updated from `9.0.7` to `10.0.0` (*recommended for .NET 10.0*)

#### CinemaTicketingSystem.Infrastructure.Authentication.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`

NuGet packages changes:
  - Microsoft.AspNetCore.Authentication.JwtBearer should be updated from `9.0.7` to `10.0.0` (*recommended for .NET 10.0*)

#### CinemaTicketingSystem.WebApi.Host.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`

NuGet packages changes:
  - Microsoft.AspNetCore.OpenApi should be updated from `9.0.5` to `10.0.0` (*recommended for .NET 10.0*)
  - Microsoft.EntityFrameworkCore.Design should be updated from `9.0.7` to `10.0.0` (*recommended for .NET 10.0*)

#### CinemaTicketingSystem.Application.Test.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`

NuGet packages changes:
  - Microsoft.AspNetCore.Mvc.Testing should be updated from `9.0.9` to `10.0.0` (*recommended for .NET 10.0*)

#### CinemaTicketingSystem.Domain.Test.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`

#### CinemaTicketingSystem.Infrastructure.DbMigrator.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`

NuGet packages changes:
  - Microsoft.Extensions.Hosting should be updated from `9.0.7` to `10.0.0` (*recommended for .NET 10.0*)
