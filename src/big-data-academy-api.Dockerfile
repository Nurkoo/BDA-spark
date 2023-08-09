FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY "BigDataAcademy.Model" "BigDataAcademy.Model/"
COPY "BigDataAcademy.Api" "BigDataAcademy.Api/"

WORKDIR /src/BigDataAcademy.Api/

RUN dotnet restore "BigDataAcademy.Api.csproj"
RUN dotnet build "BigDataAcademy.Api.csproj" --no-restore -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BigDataAcademy.Api.csproj" --no-restore -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BigDataAcademy.Api.dll"]
