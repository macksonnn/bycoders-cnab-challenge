# Estágio de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar arquivos de projeto e restaurar dependências
COPY ["src/Cnab.Api/Cnab.Api.csproj", "src/Cnab.Api/"]
COPY ["src/Cnab.Application/Cnab.Application.csproj", "src/Cnab.Application/"]
COPY ["src/Cnab.Domain/Cnab.Domain.csproj", "src/Cnab.Domain/"]
COPY ["src/Cnab.Infrastructure/Cnab.Infrastructure.csproj", "src/Cnab.Infrastructure/"]
RUN dotnet restore "src/Cnab.Api/Cnab.Api.csproj"

# Copiar todo o código fonte
COPY . .

# Build da aplicação
WORKDIR "/src/src/Cnab.Api"
RUN dotnet build "Cnab.Api.csproj" -c Release -o /app/build

# Publicar a aplicação
FROM build AS publish
RUN dotnet publish "Cnab.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 5000

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cnab.Api.dll"]

