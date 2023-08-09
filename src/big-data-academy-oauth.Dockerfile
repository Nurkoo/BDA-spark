FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY "BigDataAcademy.OAuth" "BigDataAcademy.OAuth/"

WORKDIR /src/BigDataAcademy.OAuth/

RUN dotnet restore "BigDataAcademy.OAuth.csproj"
RUN dotnet build "BigDataAcademy.OAuth.csproj" --no-restore -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BigDataAcademy.OAuth.csproj" --no-restore -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BigDataAcademy.OAuth.dll"]
