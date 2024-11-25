FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8083
EXPOSE 8083

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FIleService.Presentation/FIleService.Presentation.csproj", "FIleService.Presentation/"]
COPY ["FIleService.Application/FIleService.Application.csproj", "FIleService.Application/"]
COPY ["FIleService.Domain/FIleService.Domain.csproj", "FIleService.Domain/"]
COPY ["FIleService.Infrastructure/FIleService.Infrastructure.csproj", "FIleService.Infrastructure/"]
RUN dotnet restore "FIleService.Presentation/FIleService.Presentation.csproj"
COPY . .
WORKDIR "/src/FIleService.Presentation"
RUN dotnet build "FIleService.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FIleService.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FIleService.Presentation.dll"]
