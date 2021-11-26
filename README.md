
# Blue Crocodile

[![Build Status](https://dev.azure.com/ajfurster/Blue-Crocodile-Personal/_apis/build/status/WebApplication%20Build?branchName=master)](https://dev.azure.com/ajfurster/Blue-Crocodile-Personal/_build/latest?definitionId=3&branchName=master)

## Project overview

### `PinkPanther.BlueCrocodile.Core`

Project containing domain models and repositories.

- .net core 2.2

### `PinkPanther.BlueCrocodile.Infrastructure`

Project containing repository implementations and other implementation-specific models.

- .net core 2.2
- MongoDB Driver

### `PinkPanther.BlueCrocodile.WebApplication`

WebApplication for the manager, employees and customers.

- .net core 2.2
- C# 8
- Mongo Identity
- Mollie API

### `PinkPanther.BlueCrocodile.WebApplication.Tests`

Contains tests for `PinkPanther.BlueCrocodile.WebApplication`.

- .net core 2.2
- xunit

## Development

- Install & run MongoDB Community Server
- Clone repository
- Navigate to `PinkPanther.BlueCrocodile\PinkPanther.BlueCrocodile.WebApplication`
- Run `dotnet run`
- Create an branch for the new feature

## Deployment

### CI Azure Devops

- Create an pull request from the feature branch to the master branch
- Review & merge pull request

### Manual deployment

- Navigate to `PinkPanther.BlueCrocodile\PinkPanther.BlueCrocodile.WebApplication`
- Run `dotnet publish --configuration=Release`
- Copy files from `bin\Release\netcoreapp2.2\publish` to Kerstel webserver
