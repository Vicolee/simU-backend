# SimU-GameService

This repository contains code for the backend implementation of the Sim-U project.

## Running the application

- Clone the repository and run `dotnet build` in the [`src`](src/) directory.
- Navigate to the [API](src/SimU-GameService.Api/) directory.
- Run the `dotnet run` command to launch the application.

## Code structure

The code in this repository is structured to follow the [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) paradigm. This is a software architecture pattern that promotes separation of concerns, testability, and independence from frameworks and libraries. The architecture is designed to be adaptable to changing requirements and technologies, and to facilitate testing and code maintenance. It uses layers to separate the core business logic from technical details and allows for flexibility and maintainability. The key idea is that code dependencies point inward. This means that code in the innermost layer should have no dependencies on code in the outermost layers. Dependencies on code in the same layer or code in the inner layers is allowed. The layers in the diagram below map to the ASP.NET Core projects in this repo as follows:

| Layer | Project | Diagram layer |
| -------- | -------- | -------- |
| Domain | [Domain](src/SimU-GameService.Domain/) | Entity (yellow) |
| Application | [Application](src/SimU-GameService.Application/) | Use Cases (pink) |
| Presentation | [Api](src/SimU-GameService.Api/), [Contracts](src/SimU-GameService.Contracts/), [Infrastructure](src/SimU-GameService.Infrastructure/) | Controllers & Presenters (green) |

![Clean Architecture Overview](assets/clean_architecture.png)

## [Domain](src/SimU-GameService.Domain/)

Contains the code for the core logic of the game service. For example, the `User` class (model & methods) resides here. This project contains the code for the core components of the game that are unlikely to change as the project grows.

## [Application](src/SimU-GameService.Application/)

Contains the logic for application use cases. For example, the logic for an action such as authenticating a user into the game resides here. In this example, we might need to access the database to check and persist changes. To maintain the dependency rule that prevents code from inner layers from depending on outer components, we define interfaces that specify behavior we expect from outer components in this layer, and write the implementations in the outer layer. To follow the provided example, the [`AuthenticationService`](src/SimU-GameService.Application/Services/AuthenticationService.cs) persists changes to the database using the [`UserRepository`](src/SimU-GameService.Infrastructure/Persistence/UserRepository.cs). To work around this, we have the [`IUserRepository`](src/SimU-GameService.Application/Common/IUserRepository.cs) interface defined in this layer to allow the service to use the repository class methods to access the database.

### [Infrastructure](src/SimU-GameService.Infrastructure/)

This project contains the code that handles our application's infrastructure. For now, that is the database logic. The benefit of Clean Architecture is that we can swap the database provider or change how we access the database as need demands without breaking the rest of the application, since the inner components are only aware of the interface provided by the code in this layer, and not the actual implementation.

### [Contracts](src/SimU-GameService.Contracts/)

This is a simple class library that defines the shared *data transfer objects* (DTO) for the WebAPI. The requests define the structure of the data objects that the API expects from clients, whereas the responses define the data models sent back. This component can be thought of as an extension of the WebAPI.

### [API](src/SimU-GameService.Api/)

This is the application's interface with the Unity frontend and the bot microservice. It contains the SignalR controllers and hubs to route requests reaching the API endpoints to the correct services in the application layer and package the output into the appropriate response.
