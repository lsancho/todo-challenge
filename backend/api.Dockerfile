# syntax=docker/dockerfile:1

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:6.0 AS base
ARG configuration=Release

WORKDIR /app
COPY ["api/", "api/"]
COPY ["contracts/", "contracts/"]
RUN dotnet restore "api/TodoChallenge.Api.csproj"
RUN dotnet publish "api/TodoChallenge.Api.csproj" -c $configuration -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=base /app/publish .
USER $APP_UID
ENTRYPOINT ["./TodoChallenge.Api"]

# docker build . -f api.Dockerfile -t todo-challenge-api