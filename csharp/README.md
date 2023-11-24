# RestApi Project in C# and Entity Framework

This is a scaffold project for a REST API using C# and Entity Framework. The API is designed to be deployed on AWS using API Gateway and Lambda functions.

## Project Structure

The project is structured as follows:

- `src/RestApi`: This is the main project directory. It contains the source code for the REST API.
- `src/RestApi.Tests`: This directory contains unit tests for the REST API.
- `aws-lambda-tools-defaults.json`: This file contains default settings for AWS Lambda tools.
- `Dockerfile`: This file is used to create a Docker image for the application.

## Getting Started

To get started with this project, you will need to have .NET Core SDK installed on your machine. You will also need an AWS account to deploy the API.

## Running the Project

To run the project, navigate to the `src/RestApi` directory and run the following command:

```
dotnet run
```

This will start the application on your local machine.

## Running Tests

To run the tests, navigate to the `src/RestApi.Tests` directory and run the following command:

```
dotnet test
```

## Deploying the Project

To deploy the project, you will need to have the AWS CLI installed and configured with your AWS credentials. You can then use the `aws lambda` command to deploy the API.

## Built With

- [.NET Core](https://dotnet.microsoft.com/download)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [AWS Lambda](https://aws.amazon.com/lambda/)

## Authors

- Mudassar M - Initial work

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.