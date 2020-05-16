FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY . .
RUN dotnet restore Cognito.Web/Cognito.Web.csproj
WORKDIR /src
RUN dotnet build Cognito.Web/Cognito.Web.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Cognito.Web/Cognito.Web.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "Cognito.Web.dll"]
