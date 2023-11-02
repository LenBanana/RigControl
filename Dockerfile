FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RigControlApi/RigControlApi.csproj", "RigControlApi/"]
RUN dotnet restore "RigControlApi/RigControlApi.csproj"
COPY . .
WORKDIR "/src/RigControlApi"
RUN dotnet build "RigControlApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RigControlApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RigControlApi.dll"]
