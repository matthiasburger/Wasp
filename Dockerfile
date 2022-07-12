FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["wasp.WebApi/wasp.WebApi.csproj", "wasp.WebApi/"]
COPY ["../IronSphere.Extensions/IronSphere.Extensions/IronSphere.Extensions.csproj", "IronSphere.Extensions/"]
COPY ["wasp.Core/wasp.Core.csproj", "wasp.Core/"]
COPY ["../pythonnet/src/runtime/Python.Runtime.csproj", "runtime/"]
RUN dotnet restore "wasp.WebApi/wasp.WebApi.csproj"
COPY . .
WORKDIR "/src/wasp.WebApi"
RUN dotnet build "wasp.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "wasp.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "wasp.WebApi.dll"]
