# toolkit-api

### Getting Started

The following secrets do not show in the appsettings.json files
- ConnectionStrings:DefaultConnection
- Auth0:Domain
- Auth0:ClientSecret
- Auth0:ClientId
- Auth0:Audience
- GitHub:ClientId
- GitHub:ClientSecret

To set secrets use the command below for example
- dotnet user-secrets set "Auth0:Domain" "{value}"   

To list secrets use 
- dotnet user-secrets list

This project uses a PostgresSQL cockroachDB for its entity framework implementation

