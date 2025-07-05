FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BudgetTracker.Web/BudgetTracker.Web.csproj", "BudgetTracker.Web/"]
COPY ["BudgetTracker.Data/BudgetTracker.Data.csproj", "BudgetTracker.Data/"]
COPY ["BudgetTracker.Models/BudgetTracker.Models.csproj", "BudgetTracker.Models/"]
RUN dotnet restore "BudgetTracker.Web/BudgetTracker.Web.csproj"
COPY . .
WORKDIR "/src/BudgetTracker.Web"
RUN dotnet build "BudgetTracker.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BudgetTracker.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BudgetTracker.Web.dll"]