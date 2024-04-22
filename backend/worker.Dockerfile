# syntax=docker/dockerfile:1

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:6.0 AS base
ARG configuration=Release

WORKDIR /app
COPY ["worker/", "worker/"]
COPY ["contracts/", "contracts/"]
RUN dotnet restore "worker/TodoChallenge.Worker.csproj"
RUN dotnet publish "worker/TodoChallenge.Worker.csproj" -c $configuration -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=base /app/publish .
USER $APP_UID
ENTRYPOINT ["./TodoChallenge.Worker"]

# docker build . -f worker.Dockerfile -t todo-challenge-worker