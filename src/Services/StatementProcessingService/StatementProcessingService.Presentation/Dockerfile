FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8082
EXPOSE 8082

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StatementProcessingService.Presentation/StatementProcessingService.Presentation.csproj", "StatementProcessingService.Presentation/"]
COPY ["StatementProcessingService.Application/StatementProcessingService.Application.csproj", "StatementProcessingService.Application/"]
COPY ["StatementProcessingService.Domain/StatementProcessingService.Domain.csproj", "StatementProcessingService.Domain/"]
COPY ["StatementProcessingService.Infrastructure/StatementProcessingService.Infrastructure.csproj", "StatementProcessingService.Infrastructure/"]
RUN dotnet restore "StatementProcessingService.Presentation/StatementProcessingService.Presentation.csproj"
COPY . .
WORKDIR "/src/StatementProcessingService.Presentation"
RUN dotnet build "StatementProcessingService.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "StatementProcessingService.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StatementProcessingService.Presentation.dll"]
