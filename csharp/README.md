# RestApi Project in C# and Entity Framework

This is a scaffold project for a REST API using C# and Entity Framework. The API is designed to be deployed on AWS using API Gateway and Lambda functions. The main entity in this project is `Company`.

## Project Structure

The project is structured as follows:

- `RestApi`: This is the main project directory. It contains the source code for the REST API.
- `RestApi.Tests`: This directory contains unit tests for the REST API.

The main project `RestApi` has the following structure:

- `Controllers`: This directory contains the `CompanyController.cs` which handles HTTP requests.
- `Models`: This directory contains the `Company.cs` which is the model for the `Company` entity.
- `Services`: This directory contains services that handle business logic.

## Getting Started

To get started with this project, you will need to have .NET 5.0 SDK installed on your machine. You will also need an AWS account to deploy the API.

## Running the Project

To run the project, navigate to the `RestApi` directory and run the following command:

```bash
dotnet run