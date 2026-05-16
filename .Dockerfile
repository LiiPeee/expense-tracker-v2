# ── Stage 1: build ────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Restore com cache de camadas (copia apenas os .csproj primeiro)
COPY BudgetTracker.sln .
COPY src/BudgetTracker.WebApi/BudgetTracker.WebApi.csproj                 src/BudgetTracker.WebApi/
COPY src/BudgetTracker.Application/BudgetTracker.Application.csproj       src/BudgetTracker.Application/
COPY src/BudgetTracker.Core/BudgetTracker.Core.csproj                     src/BudgetTracker.Core/
COPY src/BudgetTracker.Infrastructure/BudgetTracker.Infrastructure.csproj src/BudgetTracker.Infrastructure/
COPY Test/Test.csproj                                                      Test/

RUN dotnet restore

# Copia o restante do código-fonte e publica
COPY . .
RUN dotnet publish src/BudgetTracker.WebApi/BudgetTracker.WebApi.csproj \
        -c Release \
        -o /app/publish \
        --no-restore

# ── Stage 2: runtime ──────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Cria usuário não-root para segurança
RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser

COPY --from=build /app/publish .

RUN chown -R appuser:appgroup /app
USER appuser

# O Render injeta a variável PORT; fallback 8080 para testes locais
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
EXPOSE 8080

ENTRYPOINT ["dotnet", "BudgetTracker.WebApi.dll"]
