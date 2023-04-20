FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ToolKit.Api.Service/ToolKit.Api.Service.csproj", "ToolKit.Api.Service/"]
RUN dotnet restore "ToolKit.Api.Service/ToolKit.Api.Service.csproj"
COPY . .
WORKDIR "/src/ToolKit.Api.Service"
RUN dotnet build "ToolKit.Api.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ToolKit.Api.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ToolKit.Api.Service.dll"]
