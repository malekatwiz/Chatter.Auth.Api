FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Chatter.Auth.Api/Chatter.Auth.Api.csproj", "Chatter.Auth.Api/"]
COPY ["Chatter.Auth.MongoIdentity/Chatter.Auth.MongoIdentity.csproj", "Chatter.Auth.MongoIdentity/"]
RUN dotnet restore "Chatter.Auth.Api/Chatter.Auth.Api.csproj"
COPY . .
WORKDIR "/src/Chatter.Auth.Api"
RUN dotnet build "Chatter.Auth.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Chatter.Auth.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Chatter.Auth.Api.dll"]
