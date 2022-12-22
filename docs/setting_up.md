# Setting Up Cloud Log

## General Dependencies

- Docker Compose
- Docker GUI (optional)
- .NET6

## MacOS

### Setup

Here is a quick guide on how I set up my development environment on MacOS.
You will also need to install nodejs and npm for the frontend.

```shell
$ brew install dotnet
$ brew install docker-compose
$ brew install --cask docker # Docker GUI

$ # Clone the repo and cd into it

$ dotnet build
$ dotnet test
```

### Running

When running in the development environment, the base URL is `http://localhost:5086`

```shell
$ docker-compose up -d
$ dotnet run --project src/LogbookServiceClient/LogbookServiceClient/LogbookServiceClient.csproj
$
$ # When finished, run the following to stop the containers
$ docker-compose down
$ # OR
$ docker-compose stop
```
