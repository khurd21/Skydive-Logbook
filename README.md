# Skydive-Logbook

## Introduction

Cloud Log is a web application that allows skydivers to track their skydives from an online platform.
Users can create an account and log and track their skydives from the web application. This project
was created as a solution to skydivers losing their physical logbooks. There is currently no online
platform that is specifically designed for skydivers to log their skydives. Cloud Log is a potential
solution to being able to track skydives online, venturing away from the traditional paper logbook.

## Status of the Project

Cloud Log is currently in the development phase. The backend is in a final state, but the frontend
is still in development.

## Purpose of the Project

The purpose of this project is to create a web application
that allows skydivers to log their skydives online, venturing
away from the traditional paper logbook. It was also created
to learn more about web development and cloud computing.

## Documentation

The documentation for Cloud Log is available within the
[docs](docs) directory. 

- [SettingUp](docs/setting_up.md)


### Dependencies / SetUp

- Docker Compose
- .NET6
- NodeJS
- NPM

#### Run the Project

- For developing / creating a local database:

```shell
cd src/LogbookServiceClient/LogbookServiceClient
dotnet ef migrations add InitialCreate
dotnet ef database update
```

- The project additionally uses DynamoDB. I set this up using Docker-Compose

```shell
$ docker-compose up -d
```

- To build the items needed, run `dotnet build` in the base directory.

- To run the project, first make sure the DynamoDB instance is running.
Then, type the following in the command line:

```shell
dotnet run --project src/LogbookServiceClient/LogbookServiceClient
```


### Example

Home Page
![Home Page](./docs/resources/home_page.png)

Logbook Page
![Logbook Page](./docs/resources/logbook_page.png)

Login Page
![Login Page](./docs/resources/login_page.png)
