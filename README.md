# Azure CosmosDB Repository

Generic Repository for Azure CosmosDB using SQL API

## Overview
This application is a .NET Core 2.1 class library written in C#.  There are two projects. One for the class library and another for unit tests.

- AzureCosmosDBRepository
- AzureCosmosDBRepository.Tests

The purpose of this application is to provide an example of working with Azure CosmosDB using the SQL API model.  

## Running The Tests

From the command line, run the following in the root of the repository:

    dotnet test .\AzureCosmosDBRepository.Tests\AzureCosmosDBRepository.Tests.csproj --verbosity normal

This will build the solution, then execute the tests.  You may also build using the following commands.

	dotnet clean
	dotnet build

The build step will fetch all nuget packages required of the projects.  It is not necessary, but to manually restore the packages simply execute the following command:

    dotnet restore

You may also open the solution file (AzureCosmosDBRepository.sln) with Visual Studio and run tests.

## Author
- Jonathan E. Ross
